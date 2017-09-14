using System.Runtime.CompilerServices;
using TEDExporter.DTO;
using TEDExporter.Map;

namespace TEDExporter
{
    public static class TedToLeaKitMapper
    {
        public static ImportLeanKit GetDTOForTED(ExportTED ted)
        {
            ImportLeanKit t = new ImportLeanKit();
            t.Card_Description = ted.Intitule;
            t.Card_Title = $"TED {ted.Numero}";

            DomaineTED domaine = GetDomaine(ted);
            NatureTED nature = GetNature(ted);
            switch (nature)
            {
                case NatureTED.Suivi_Production:
                    t.Card_Type = Card.TYPE_SUIVI;
                    //to control the Lane
                    switch (domaine)
                    {
                        case DomaineTED.eSRC:
                            t.Lane_Id = BoardESrc.ID_TACHE.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_TACHE.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_TACHE.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
                case NatureTED.Evolution:
                    t.Card_Type = Card.TYPE_EVOL;
                    //to control the Lane
                    switch (domaine)
                    {
                        case DomaineTED.eSRC:
                            t.Lane_Id = BoardESrc.ID_EVOLUTION.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_EVOLUTION.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_EVOLUTION.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
                case NatureTED.Tache:
                    t.Card_Type = Card.TYPE_TACHES;
                    //to control the Lane
                    switch (domaine)
                    {
                        case DomaineTED.eSRC:
                            t.Lane_Id = BoardESrc.ID_TACHE.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_TACHE.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_TACHE.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
                case NatureTED.Imprevu:
                    // controles sur la version de la carte ou un PAC 
                    if (ted.VersionPrevue.Contains("PAC"))
                    {
                        t.Card_Type = Card.TYPE_PAC;
                    }
                    else
                    {
                        if (ted.VersionPrevue.Contains("4.20"))
                        {
                            t.Card_Type = Card.TYPE_VINGT;
                        }
                        else if (ted.VersionPrevue.Contains("4.21"))
                        {
                            t.Card_Type = Card.TYPE_VINGTUN;
                        }
                        else if (ted.VersionPrevue.Contains("4.22"))
                        {
                            t.Card_Type = Card.TYPE_VINGTDEUX;
                        }
                        else
                        {
                            t.Card_Type = ted.VersionPrevue;
                        }
                    }
                    //to control the Lane
                    switch (domaine)
                    {
                        case DomaineTED.eSRC:
                            t.Lane_Id = BoardESrc.ID_IMPREVU.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_IMPREVU.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_IMPREVU.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
                default:
                    t.Card_Type = "INCONNU";
                    break;
            }
            return t;
        }


        public static DomaineTED GetDomaine(ExportTED ted)
        {
            if (ted.SousSysteme == "Prestations FSS")
            {
                return DomaineTED.FSS;
            }else
            if (ted.SousSysteme == "eSRC" || ted.Intitule.ToUpper().StartsWith("ESRC"))
            {
                return DomaineTED.eSRC;
            }else
            if (ted.SousSysteme == "ServicesCommuns")
            {
                return DomaineTED.Commun;
            }else
            if (ted.SousSysteme.StartsWith("Actuaria"))
            {
                return DomaineTED.Tauri;
            }else
            {
                return DomaineTED.Inconnu;
            }
        }

        public static NatureTED GetNature(ExportTED ted)
        {
            if (ted.Intitule.ToUpper().StartsWith("[SUIVI") || ted.Intitule.ToUpper().Contains("SUIVI DE PROD"))
            {
                return NatureTED.Suivi_Production;
            }
            else if (ted.Type == "Evolution")
            {
                return NatureTED.Evolution;
            }
            else if (ted.Type == "Imprévu")
            {
                return NatureTED.Imprevu;
            }
            else if (ted.Type == "Tâche")
            {
                return NatureTED.Tache;
            }
            else
            {
                return NatureTED.Inconnu;
            }
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
