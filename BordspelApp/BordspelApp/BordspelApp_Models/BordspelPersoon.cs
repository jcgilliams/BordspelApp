using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class BordspelPersoon : BasisKlasse
    {
        public int id { get; set; }
        public int bordspelId { get; set; }
        public int persoonId { get; set; }
        public int rolId { get; set; }
        public Persoon persoon { get; set; }
        public Rol rol { get; set; }
        public Bordspel bordspel { get; set; }

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
                if (columnName == nameof(persoonId) && persoonId <= 0)
                {
                    return "De persoonId moet een getal boven nul zijn.";
                }
                if (columnName == nameof(rolId) && rolId <=0)
                {
                    return "De rolId moet een getal boven nul zijn.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is BordspelPersoon bordspelPersoon &&
                   bordspelId == bordspelPersoon.bordspelId &&
                   persoonId == bordspelPersoon.persoonId &&
                   rolId == bordspelPersoon.rolId;
        }

        public override int GetHashCode()
        {
            int hashCode = 12345;
            hashCode = hashCode * -12345 + id.GetHashCode();
            hashCode = hashCode * -12345 + bordspelId.GetHashCode();
            hashCode = hashCode * -12345 + persoonId.GetHashCode();
            hashCode = hashCode * -12345 + rolId.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"{this.persoon}";
        }
    }
}
