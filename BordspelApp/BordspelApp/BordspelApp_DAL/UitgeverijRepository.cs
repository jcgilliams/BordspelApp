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
    public class UitgeverijRepository : BaseRepository, IUitgeverijRepository
    {
        // ophalen van alle uitgeverijen
        public IEnumerable<Uitgeverij> OphalenUitgeverijen()
        {
            string sql = "SELECT * FROM ProjectBGG.Uitgeverij ORDER BY naam; ";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Uitgeverij>(sql);
            };
        }
        // verwijder uitgeverij van bordspel (delete)
        public bool VerwijderUitgeverijVanBordspel(int uitgeverij, int bordspel)
        {
            string sql = @"DELETE FROM ProjectBGG.BordspelUitgeverij
                            WHERE uitgeverijId = @uitgeverijId AND bordspelId = @bordspelId;";

            var parameters = new
            {
                @uitgeverijId = uitgeverij,
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
        // nieuwe uitgeverij aanmaken (insert)
        public bool VoegUitgeverijToeAanDatabank(Uitgeverij uitgeverij)
        {
            string sql = @"INSERT INTO [ProjectBGG].[Uitgeverij] (naam, beschrijving, website, persoonId, land)   
                            VALUES (@naam, NULL,NULL, NULL, @land);";

            var parameters = new
            {
                @naam = uitgeverij.naam,
                @land = uitgeverij.land
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
        // uitgeverij aanpassen (update)
        public bool UpdateUitgeverij(Uitgeverij uitgeverij)
        {
            string sql = @"UPDATE [ProjectBGG].[Uitgeverij] SET naam = @naam, 
                                            land = @land
                          WHERE id = @uitgeverijid";

            var parameters = new
            {
                @uitgeverijid = uitgeverij.id,
                @naam = uitgeverij.naam,
                @land = uitgeverij.land
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
        // uitgeverij toevoegen aan bordspel (insert)
        public bool VoegUitgeverijToeAanBordspel(BordspelUitgeverij bordspelUitgeverij)
        {
            string sql = @"INSERT INTO [ProjectBGG].[BordspelUitgeverij] (bordspelId, uitgeverijId, taal)   
                            VALUES (@bordspelId, @uitgeverijId,null);";

            var parameters = new
            {
                @bordspelId = bordspelUitgeverij.bordspelId,
                @uitgeverijId = bordspelUitgeverij.uitgeverij.id
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
