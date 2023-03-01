using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class BordspelGebruiker : BasisKlasse
    {
        public int id { get; set; }
        public int bordspelId { get; set; }
        public int gebruikerId { get; set; }
        public int? rating { get; set; }
        public Bordspel Bordspel { get; set; }
        public Gebruiker Gebruiker { get; set; }
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
                if (columnName == nameof(gebruikerId) && gebruikerId <= 0)
                {
                    return "De gebruikersId moet een getal boven nul zijn.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is BordspelGebruiker bordspelGebruiker &&
                   bordspelId == bordspelGebruiker.bordspelId &&
                   gebruikerId == bordspelGebruiker.gebruikerId;
        }

        public override int GetHashCode()
        {
            int hashCode = 12345;
            hashCode = hashCode * -12345 + id.GetHashCode();
            hashCode = hashCode * -12345 + bordspelId.GetHashCode();
            hashCode = hashCode * -12345 + gebruikerId.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"{Bordspel}";
        }
    }
}
