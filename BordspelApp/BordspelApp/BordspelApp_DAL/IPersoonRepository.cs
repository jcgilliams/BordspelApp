using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;

namespace BordspelApp_DAL
{
    public interface IPersoonRepository
    {
        public IEnumerable<Persoon> OphalenPersonen();
        public bool VerwijderPersoonVanBordspel(int persoon, int bordspel);
        public bool VoegPersoonToeAanDatabank(Persoon persoon);
        public bool VoegPersoonToeAanBordspel(BordspelPersoon bordspelPersoon);
        public bool UpdatePersoon(Persoon persoon);
    }
}
