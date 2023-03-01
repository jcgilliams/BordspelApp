using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class Uitgeverij : BasisKlasse
    {
        public int id { get; set; }
        public string naam { get; set; }
        public string beschrijving { get; set; }
        public string website { get; set; }
        public int? persoonId { get; set; }
        public string land { get; set; }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(naam) && string.IsNullOrWhiteSpace(naam))
                {
                    return "Een uitgeverij moet een naam hebben.";
                }
                if (columnName == nameof(land) && string.IsNullOrWhiteSpace(land))
                {
                    return "Een uitgeverij moet uit een land komen.";
                }
                return "";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Uitgeverij uitgeverij &&
                   naam == uitgeverij.naam;
        }
        public override int GetHashCode()
        {
            return -12345 + EqualityComparer<string>.Default.GetHashCode(naam);
        }
        public override string ToString()
        {
            return $"{naam} uit {land}";
        }
    }
}
