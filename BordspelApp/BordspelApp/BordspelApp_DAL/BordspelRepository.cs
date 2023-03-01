using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace BordspelApp_DAL
{
    public class BordspelRepository : BaseRepository, IBordspelRepository
    {
        // testen of ik spellen kon laden; verder niet gebruikt
        public IEnumerable<Bordspel> OphalenBordspellen()
        {
            string sql = "SELECT * FROM ProjectBGG.Bordspel ORDER BY naam";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Bordspel>(sql);
            }
        }

        // ophalen van alle spellen die niet in collectie zitten van in dit geval gebruiker 2
        public IEnumerable<Bordspel> OphalenBordspellenNietInCollectie()
        {
            string sql = @"SELECT Bordspel.*, '' as 'SplitCol'
                            FROM ProjectBGG.Bordspel
                            WHERE id NOT IN
                                (SELECT bordspelId
                                FROM ProjectBGG.BordspelGebruiker
                                WHERE gebruikerId = 2)
                            ORDER BY Bordspel.naam; ";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var debugVar = db.Query<Bordspel, BordspelGebruiker, Bordspel>(
                    sql,
                    (bordspel, bordspelgebruiker) =>
                    {
                        bordspelgebruiker.Bordspel = bordspel;
                        return bordspel;
                    },
                    splitOn: "SplitCol"
                );
                return debugVar;
            }
        }
        // voor filter in collectionview
        public IEnumerable<Bordspel> ZoekenBordspellen(string bordspel)
        {
            string sql = @"SELECT *, '' as 'SplitCol'
                            FROM ProjectBGG.Bordspel
                            WHERE id  NOT IN
                                (SELECT bordspelId
                                FROM ProjectBGG.BordspelGebruiker
                                WHERE gebruikerId = 2 ) AND Bordspel.naam LIKE '%' + @Bordspel + '%'
                            ORDER BY Bordspel.naam; ";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var debugVar = db.Query<Bordspel, BordspelGebruiker, Bordspel>(
                    sql,
                    (bordspel, bordspelgebruiker) =>
                    {
                        bordspelgebruiker.Bordspel = bordspel;
                        return bordspel;
                    },
                    new { bordspel = bordspel },
                    splitOn: "SplitCol"
                );
                return debugVar;
            }
        }

        // eerste werkende versie die enkel bordspel binnenhaalt

        //public Bordspel ZoekenBordspelViaId(int id)
        //{
        //    string sql = @"SELECT * FROM ProjectBGG.Bordspel WHERE id = @id";

        //    var parameters = new
        //    {
        //        @id = id
        //    };
        //    using (IDbConnection db = new SqlConnection(ConnectionString))
        //    {
        //        return db.QuerySingleOrDefault<Bordspel>(sql, parameters);
        //    }
        //}
        
        // herschreven met joins van alle tabellen
        public Bordspel ZoekenBordspelViaId(int id)
        {
            string sql = @"SELECT * 
                            FROM ProjectBGG.Bordspel 
                            LEFT JOIN ProjectBGG.BordspelPersoon ON Bordspel.id = BordspelPersoon.bordspelId 
                            LEFT JOIN ProjectBGG.BordspelUitgeverij ON Bordspel.id = BordspelUitgeverij.bordspelId
                            LEFT JOIN ProjectBGG.Uitgeverij ON BordspelUitgeverij.uitgeverijId = Uitgeverij.id
                            LEFT JOIN ProjectBGG.Persoon ON BordspelPersoon.persoonId = Persoon.id
                            LEFT JOIN ProjectBGG.Rol ON BordspelPersoon.rolId = Rol.id
                            WHERE Bordspel.id = @id";

            var parameters = new
            {
                @id = id
            };
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var bordspellen = db.Query<Bordspel, BordspelPersoon, BordspelUitgeverij, Uitgeverij, Persoon, Rol, Bordspel>(
                    sql,
                    (bordspel, bordspelPersoon, bordspelUitgeverij, uitgeverij, persoon, rol) =>
                    {
                        
                        if (bordspelPersoon != null)
                        {
                            bordspel.BordspelPersoon = new List<BordspelPersoon>() { bordspelPersoon };
                            bordspelPersoon.persoon = persoon;
                            bordspelPersoon.rol = rol;
                            bordspelPersoon.bordspel = bordspel;
                        }                        
                        
                        if (bordspelUitgeverij != null)
                        {
                            bordspel.BordspelUitgeverij = new List<BordspelUitgeverij>() { bordspelUitgeverij };
                            bordspelUitgeverij.uitgeverij = uitgeverij;  
                        }                                              

                        return bordspel;
                    },
                    parameters
                );

                var gegroepeerdeBordspellen = GroepeerBordspel(bordspellen);
                return gegroepeerdeBordspellen.SingleOrDefault();
            }
        }
        // groeperen bordspellen en dubbels eruit
        private static ICollection<Bordspel> GroepeerBordspel(IEnumerable<Bordspel> bordspel)
        {
            if (bordspel.Count() > 1)
            {
                var gegroepeerd = bordspel.GroupBy(b => b.id);

                return gegroepeerd.Select(g =>
                {
                    var bordspel = g.First();
                    if(bordspel.BordspelPersoon.Count() > 0)
                    {
                        bordspel.BordspelPersoon = g.Select(b => b.BordspelPersoon.Single()).ToHashSet();
                    }

                    if (bordspel.BordspelUitgeverij.Count() > 0)
                    {
                        bordspel.BordspelUitgeverij = g.Select(b =>b.BordspelUitgeverij.Single()).ToHashSet();
                    }                        
                    return bordspel;
                }).ToList();
            }
            return bordspel.ToList();
        }

        // updaten van bordspel (update)
        public bool UpdateBordspel(Bordspel bordspel)
        {
            string sql = @"UPDATE [ProjectBGG].[Bordspel] SET naam = @naam, 
                                            jaar = @jaar,
                                            beschrijving = @beschrijving,
                                            minAantalSpelers = @minAantalSpelers,
                                            maxAantalSpelers = @maxAantalSpelers,
                                            minSpeeltijd = @minSpeeltijd,
                                            maxSpeeltijd = @maxSpeeltijd,
                                            leeftijd = @leeftijd
                          WHERE id = @bordspelid";

            var parameters = new
            {
                @bordspelid = bordspel.id,
                @naam = bordspel.naam,
                @jaar = bordspel.jaar,
                @beschrijving = bordspel.beschrijving,
                @minAantalSpelers = bordspel.minAantalSpelers,
                @maxAantalSpelers = bordspel.maxAantalSpelers,
                @minSpeeltijd = bordspel.minSpeeltijd,
                @maxSpeeltijd = bordspel.maxSpeeltijd,
                @leeftijd = bordspel.leeftijd
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute(sql, parameters);
                if (affectedRows == 1)
                {
                    return true;
                }
            }
            return false;
        }

        // dank aan Sebastiaan Henau om OUTPUT inserted.* én db.Query toe te voegen zodat ik na aanmaak nieuw spel ineens ontwerper kan toevoegen ipv naar Home te gaan
        // nieuw spel toevoegen maar vooral zorgen dat er id teruggegeven wordt zodat de andere schermen zoals artiest, ontwerper, uitgeverij ook ineens geladen worden en kunnen aangevuld worden (insert)
        public Bordspel VoegBordspelToeAanDatabank(Bordspel bordspel)
        {
            string sql = @"INSERT INTO [ProjectBGG].[Bordspel] (naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd) 
                            OUTPUT inserted.*
                            VALUES (@naam, @jaar,@beschrijving,@minAantalSpelers, @maxAantalSpelers, @minSpeeltijd, @maxSpeeltijd, @leeftijd);";

            var parameters = new
            {
                @naam = bordspel.naam,
                @jaar = bordspel.jaar,
                @beschrijving = bordspel.beschrijving,
                @minAantalSpelers = bordspel.minAantalSpelers,
                @maxAantalSpelers = bordspel.maxAantalSpelers,
                @minSpeeltijd = bordspel.minSpeeltijd,
                @maxSpeeltijd = bordspel.maxSpeeltijd,
                @leeftijd = bordspel.leeftijd
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Bordspel>(sql, parameters).SingleOrDefault();           
            }
        }
    }
}
