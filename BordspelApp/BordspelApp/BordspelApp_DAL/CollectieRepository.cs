using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace BordspelApp_DAL
{
    public class CollectieRepository : BaseRepository, ICollectieRepository
    {
        // ophalen van alle spellen die in dit geval in collectie zitten van gebruiker 2
        public IEnumerable<BordspelGebruiker> OphalenCollectie()
        {
            string sql = @"SELECT BordspelGebruiker.* , '' as 'SplitCol', Bordspel.*
                                FROM ProjectBGG.BordspelGebruiker
                                INNER JOIN ProjectBGG.Bordspel ON BordspelGebruiker.bordspelId = Bordspel.id
                                WHERE gebruikerId = 2
                                ORDER BY naam;";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var debugVar = db.Query<BordspelGebruiker, Bordspel, BordspelGebruiker>(
                    sql,
                    (bordspelGebruiker, bordspel) =>
                    {
                        bordspelGebruiker.Bordspel = bordspel;
                        return bordspelGebruiker;
                    },
                    splitOn: "SplitCol"
                );
                return debugVar;
            }
        }

        // voor filter in collectionview
        public IEnumerable<BordspelGebruiker> ZoekenBordspellen(string bordspel)
        {
            string sql = @"SELECT BordspelGebruiker.* , '' as 'SplitCol', Bordspel.*
                                FROM ProjectBGG.BordspelGebruiker
                                INNER JOIN ProjectBGG.Bordspel ON BordspelGebruiker.bordspelId = Bordspel.id
                                WHERE gebruikerId = 2 AND Bordspel.naam LIKE '%' + @Bordspel + '%'
                                ORDER BY naam;";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var debugVar = db.Query<BordspelGebruiker, Bordspel, BordspelGebruiker>(
                    sql,
                    (bordspelGebruiker, bordspel) =>
                    {
                        bordspelGebruiker.Bordspel = bordspel;
                        return bordspelGebruiker;
                    },
                    new { bordspel = bordspel },
                    splitOn: "SplitCol"
                );
                return debugVar;
            }
        }

        // spel in collectie zetten (bij gebruiker 2 gezien deze id standaard ingevuld is! (insert)
        public bool ZetInCollectie(int id)
        {
            string sql = @"INSERT INTO ProjectBGG.BordspelGebruiker (bordspelId,gebruikerId,rating)
	                        VALUES (@bordspelId,2,null)";

            var parameters = new
            {
                @bordspelId = id

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

        // uit de collectie halen van een bordspel (delete)
        public bool HaalUitCollectie(int id)
        {
            string sql = @"DELETE FROM ProjectBGG.BordspelGebruiker
                            WHERE bordspelId = @bordspelId;";

            var parameters = new
            {
                @bordspelId = id
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
        // bordspel verwijderen uit de databank (delete)
        public bool VerwijderUitDatabank(int id)
        {
            string sql = @"DELETE FROM ProjectBGG.BordspelGebruiker
                            WHERE bordspelId = @bordspelId;                          
                           DELETE FROM ProjectBGG.BordspelPersoon
                            WHERE bordspelId = @bordspelId;
                           DELETE FROM ProjectBGG.BordspelUitgeverij
                            WHERE bordspelId = @bordspelId;
                           DELETE FROM ProjectBGG.Bordspel
                            WHERE id = @bordspelId;";


            var parameters = new
            {
                @bordspelId = id
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
    }
}
