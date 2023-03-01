using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;

namespace BordspelApp_DAL
{
    public interface IUitgeverijRepository
    {
        public IEnumerable<Uitgeverij> OphalenUitgeverijen();
        public bool VerwijderUitgeverijVanBordspel(int uitgeverij, int bordspel);
        public bool VoegUitgeverijToeAanDatabank(Uitgeverij uitgeverij);
        public bool UpdateUitgeverij(Uitgeverij uitgeverij);
        public bool VoegUitgeverijToeAanBordspel(BordspelUitgeverij bordspelUitgeverij);
    }
}
