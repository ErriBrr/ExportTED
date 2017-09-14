using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEDExporter.Map
{
    public abstract class Board
    {
        public int ID_SUIVI;
        public int ID_EVOLUTION;
        public int ID_TACHE;
        public int ID_IMPREVU;
        public int ID_DEFAUT;

        public string TITRE_SUIVI;
        public string TITRE_EVOLUTION;
        public string TITRE_TACHE;
        public string TITRE_IMPREVU;
        public string TITRE_DEFAUT;
    }
}
