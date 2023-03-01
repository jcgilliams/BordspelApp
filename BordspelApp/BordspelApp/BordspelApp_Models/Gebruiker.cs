using System;
using System.Collections.Generic;
using System.Text;

namespace BordspelApp_Models
{
    public class Gebruiker : BasisKlasse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string pasword { get; set; }

        public override string this[string columnName] 
        {
            get
            {
                if (columnName == nameof(id) && id <= 0)
                {
                    return "id moet een getal boven nul zijn.";
                }
                if (columnName == nameof(username) && string.IsNullOrWhiteSpace(username))
                {
                    return "Een gebruiker moet een naam hebben.";
                }
                if (columnName == nameof(pasword) && string.IsNullOrWhiteSpace(pasword))
                {
                    return "Een gebruiker moet een paswoord hebben.";
                }
                return "";
            }
        }
        public override bool Equals(object obj)
        {
            return obj is Gebruiker gebruiker &&
                   username == gebruiker.username;
        }
        public override int GetHashCode()
        {
            return -12345 + EqualityComparer<string>.Default.GetHashCode(username);
        }
        public override string ToString()
        {
            return $"{username}";
        }
    }
}
