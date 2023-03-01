using System;
using System.Collections.Generic;
using System.Text;
using BordspelApp_Models;

namespace BordspelApp_DAL
{
    public interface IBordspelRepository
    {
        public IEnumerable<Bordspel> OphalenBordspellen();
        public IEnumerable<Bordspel> OphalenBordspellenNietInCollectie();
        public IEnumerable<Bordspel> ZoekenBordspellen(string bordspel);
        public Bordspel ZoekenBordspelViaId(int id);
        public bool UpdateBordspel(Bordspel bordspel);
        public Bordspel VoegBordspelToeAanDatabank(Bordspel bordspel);
    }
}
