using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEDExporter.Map
{
    public class BoardESrc : Board
    {
        public const int ID_IMPREVU = 540844401;
        public const int ID_DEFAUT = 540844405;
        public const int ID_TACHE = 540844409;
        public const int ID_EVOLUTION = 540844413;
        public const int ID_SUIVI = 540844417;

        public const string TITRE_IMPREVU = "backlog:ted imprévus";
        public const string TITRE_DEFAUT = "backlog:tfs";
        public const string TITRE_TACHE = "backlog:ted tâches";
        public const string TITRE_EVOLUTION = "backlog:ted evolutions";
        public const string TITRE_SUIVI = "backlog:ted suivi production";
    }
}
