using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class Rol : BasisKlasse
    {
        public int id { get; set; }
        public string beschrijving { get; set; }
        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(id) && id <= 0)
                {
                    return "id moet een getal boven nul zijn.";
                }
                if (columnName == nameof(beschrijving) && string.IsNullOrWhiteSpace(beschrijving))
                {
                    return "Een rol moet een beschrijving hebben.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is Rol rol &&
                   beschrijving == rol.beschrijving;
        }
        public override int GetHashCode()
        {
            return -12345 + EqualityComparer<string>.Default.GetHashCode(beschrijving);
        }
        public override string ToString()
        {
            return $"{beschrijving}";
        }
    }
}
