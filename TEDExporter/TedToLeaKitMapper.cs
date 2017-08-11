using TEDExporter.DTO;

namespace TEDExporter
{
    public static class TedToLeaKitMapper
    {
        public static string CARD_TYPE_TACHES = "512063320";
        public static string CARD_TYPE_EVOL = "512063321";
        public static string CARD_TYPE_SUIVI = "519685034";

        public static string LANE_SUIVI = "521748899";
        public static string LANE_EVOL_ESRC = "521517640";
        public static string LANE_EVOL_TAURI = "516686007";
        public static string LANE_EVOL_FSS = "516686010";

        public static string LANE_TACHE_ESRC = "512726405";
        public static string LANE_TACHE_TAURI = "512720054";
        public static string LANE_TACHE_FSS = "512720055";

        public static string LANE_IMPREVU_ESRC = "512726407";
        public static string LANE_IMPREVU_TAURI = "512720056";
        public static string LANE_IMPREVU_FSS = "512720057";

        public static ImportLeanKit GetDTOForTED(ExportTED ted)
        {
            var t = new ImportLeanKit();
            t.Card_Description = ted.Intitule;
            t.Card_Title = $"TED {ted.Numero}";
            var domaine = GetDomaine(ted);
            var nature = GetNature(ted);
            switch (nature)
            {
                case NatureTED.Suivi_Production:
                    t.Lane_Id = LANE_SUIVI;
                    t.Card_Type = CARD_TYPE_SUIVI;
                    break;
                case NatureTED.Evolution:
                    t.Card_Type = CARD_TYPE_EVOL;
                    switch (domaine)
                    {
                       
                        case DomaineTED.eSRC:
                            t.Lane_Id = LANE_EVOL_ESRC;
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = LANE_EVOL_TAURI;
                            break;
                        case DomaineTED.FSS:
                        default:
                            t.Lane_Id = LANE_EVOL_FSS;
                            break;
                    }
                    break;
                case NatureTED.Tache:
                    t.Card_Type = CARD_TYPE_TACHES;
                    switch (domaine)
                    {
                       
                        case DomaineTED.eSRC:
                            t.Lane_Id = LANE_TACHE_ESRC;
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = LANE_TACHE_TAURI;
                            break;
                        case DomaineTED.FSS:
                        default:
                            t.Lane_Id = LANE_TACHE_FSS;
                            break;
                    }
                    break;
                case NatureTED.Imprevu:
                default:
                    t.Card_Type = CARD_TYPE_TACHES;
                    switch (domaine)
                    {
                        
                        case DomaineTED.eSRC:
                            t.Lane_Id = LANE_IMPREVU_ESRC;
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = LANE_IMPREVU_TAURI;
                            break;
                        case DomaineTED.FSS:
                        default:
                            t.Lane_Id = LANE_IMPREVU_FSS;
                            break;
                    }
                    break;

            }
            return t;
        
        }


        public static DomaineTED GetDomaine(ExportTED ted)
        {
            if (ted.SousSysteme == "Prestations FSS")
            {
                return DomaineTED.FSS;
            }
            if(ted.SousSysteme == "eSRC" || ted.Intitule.ToUpper().StartsWith("ESRC"))
            {
                return DomaineTED.eSRC;
            }
            if(ted.SousSysteme == "ServicesCommuns")
            {
                return  DomaineTED.Commun;
            }
            if (ted.SousSysteme.StartsWith("Actuaria"))
            {
                return DomaineTED.Tauri;
            }
            return DomaineTED.Inconnu;

        }

        public static NatureTED GetNature(ExportTED ted)
        {
            if (ted.Intitule.ToUpper().StartsWith("[SUIVI") || ted.Intitule.ToUpper().Contains("SUIVI DE PROD"))
            {
                return NatureTED.Suivi_Production;
            }
            if(ted.Type == "Evolution")
            {
                return NatureTED.Evolution;
            }

            if(ted.Type == "Imprévu")
            {
                return NatureTED.Imprevu;
            }

            if(ted.Type == "Tâche")
            {
                return NatureTED.Tache;
            }
            return NatureTED.Inconnu;
        }
    }

    public enum DomaineTED
    {
        FSS,
        eSRC,
        Commun,
        Tauri,
        Inconnu
    }

    public enum NatureTED
    {
        Suivi_Production,
        Evolution,
        Imprevu,
        Tache,
        Inconnu
    }
}
