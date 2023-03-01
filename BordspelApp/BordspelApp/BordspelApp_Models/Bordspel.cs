using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class Bordspel : BasisKlasse
    {
        public int id { get; set; }
        public string naam { get; set; }
        public int jaar { get; set; }
        public string beschrijving { get; set; }
        public int? minAantalSpelers { get; set; }
        public int? maxAantalSpelers { get; set; }
        public int? minSpeeltijd { get; set; }
        public int? maxSpeeltijd { get; set; }
        public int? leeftijd { get; set; }
        public ICollection<BordspelPersoon> BordspelPersoon { get; set; }
        public ICollection<BordspelUitgeverij> BordspelUitgeverij { get; set; }

        public Bordspel()
        {
            BordspelPersoon = new List<BordspelPersoon>();
            BordspelUitgeverij = new List<BordspelUitgeverij>();
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(naam) && string.IsNullOrWhiteSpace(naam))
                {
                    return "Een bordspel moet een naam hebben.";
                }
                if (columnName == nameof(jaar) && jaar < 1900)
                {
                    return "Een bordspel moet een releasejaar hebben en dit moet na 1900 zijn.";
                }
                return "";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Bordspel bordspel &&
                   naam == bordspel.naam;
        }
        public override int GetHashCode()
        {
            return -12345 + EqualityComparer<string>.Default.GetHashCode(naam);
        }
        public override string ToString()
        {
            return $"{naam}";
        }
    }
}
