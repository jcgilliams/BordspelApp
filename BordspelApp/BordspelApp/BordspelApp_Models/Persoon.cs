using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class Persoon : BasisKlasse
    {
        public int id { get; set; }
        public string naam { get; set; }
        public string voornaam { get; set; }
        public string beschrijving { get; set; }
        public int? gebruikerId { get; set; }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(naam) && string.IsNullOrWhiteSpace(naam))
                {
                    return "Een persoon moet een naam hebben.";
                }
                if (columnName == nameof(voornaam) && string.IsNullOrWhiteSpace(voornaam))
                {
                    return "Een persoon moet een voornaam hebben.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is Persoon persoon &&
                   naam == persoon.naam &&
                   voornaam == persoon.voornaam;
        }
        public override int GetHashCode()
        {
            int hashCode = 12345;
            hashCode = hashCode * -12345 + EqualityComparer<string>.Default.GetHashCode(naam);
            hashCode = hashCode * -12345 + EqualityComparer<string>.Default.GetHashCode(voornaam);
            return hashCode;
        }
        public override string ToString()
        {
            return $"{naam} {voornaam}";
        }
    }
}
