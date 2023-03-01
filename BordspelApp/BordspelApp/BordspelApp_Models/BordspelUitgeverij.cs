using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class BordspelUitgeverij : BasisKlasse
    {
        public int id { get; set; }
        public int bordspelId { get; set; }
        public int uitgeverijId { get; set; }
        public string taal { get; set; }
        public Bordspel bordspel { get; set; }
        public Uitgeverij uitgeverij { get; set; }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(id) && id <= 0)
                {
                    return "id moet een getal boven nul zijn.";
                }
                if (columnName == nameof(bordspelId) && bordspelId <= 0)
                {
                    return "De bordspelId moet een getal boven nul zijn.";
                }
                if (columnName == nameof(uitgeverijId) && uitgeverijId <= 0)
                {
                    return "De uitgeverijId moet een getal boven nul zijn.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is BordspelUitgeverij bordspelUitgeverij &&
                   bordspelId == bordspelUitgeverij.bordspelId &&
                   uitgeverijId == bordspelUitgeverij.uitgeverijId;
        }

        public override int GetHashCode()
        {
            int hashCode = 12345;
            hashCode = hashCode * -12345 + id.GetHashCode();
            hashCode = hashCode * -12345 + bordspelId.GetHashCode();
            hashCode = hashCode * -12345 + uitgeverijId.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"{uitgeverij}";
        }
    }
}
