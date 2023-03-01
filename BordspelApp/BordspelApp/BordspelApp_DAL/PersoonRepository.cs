using BordspelApp_Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace BordspelApp_DAL
{
    public class PersoonRepository : BaseRepository, IPersoonRepository
    {
        // ophalen van alle personen die geen uitgeverij zijn!
        public IEnumerable<Persoon> OphalenPersonen()
        {
            string sql = "SELECT * FROM ProjectBGG.Persoon WHERE voornaam != 'Uitgeverij' ORDER BY naam; ";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Persoon>(sql);
            };
        }
        // verwijderen persoon van bordspel (delete)
        public bool VerwijderPersoonVanBordspel(int persoon, int bordspel)
        {
            string sql = @"DELETE FROM ProjectBGG.BordspelPersoon
                            WHERE persoonId = @persoonId AND bordspelId = @bordspelId";

            var parameters = new
            {
                @persoonId = persoon,
                @bordspelId = bordspel
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
        // persoon toevoegen aan databank (insert)
        public bool VoegPersoonToeAanDatabank(Persoon persoon)
        {
            string sql = @"INSERT INTO [ProjectBGG].[Persoon] (naam, voornaam, beschrijving, gebruikerId)   
                            VALUES (@naam, @voornaam,NULL,NULL);";

            var parameters = new
            {
                @naam = persoon.naam,
                @voornaam = persoon.voornaam
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute(sql, parameters);
                if(affectedRows == 1)
                {
                    return true;
                }
            }
            return false;
        }
        // persoon toevoegen aan bordspel (insert)
        public bool VoegPersoonToeAanBordspel(BordspelPersoon bordspelPersoon)
        {
            string sql = @"INSERT INTO [ProjectBGG].[BordspelPersoon] (bordspelId, persoonId, rolId)   
                            VALUES (@bordspelId, @persoonId,@rolId);";

            var parameters = new
            {
                @bordspelId = bordspelPersoon.bordspelId,
                @persoonId = bordspelPersoon.persoonId,
                @rolId = bordspelPersoon.rolId
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
        // persoon updaten (en dus aanpassen) (update)
        public bool UpdatePersoon(Persoon persoon)
        {
            string sql = @"UPDATE [ProjectBGG].[Persoon] SET naam = @naam, 
                                            voornaam = @voornaam
                          WHERE id = @persoonid";
            
            var parameters = new
            {
                @persoonid = persoon.id,
                @naam = persoon.naam,
                @voornaam = persoon.voornaam
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
