using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;

namespace BordspelApp_DAL
{
    public interface ICollectieRepository
    {
        public IEnumerable<BordspelGebruiker> OphalenCollectie();
        public IEnumerable<BordspelGebruiker> ZoekenBordspellen(string bordspel);
        public bool ZetInCollectie(int id);
        public bool HaalUitCollectie(int id);
        public bool VerwijderUitDatabank(int id);
    }
}
