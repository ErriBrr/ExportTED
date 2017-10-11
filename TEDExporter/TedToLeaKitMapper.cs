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
            t.Card_Title = $"{ted.Numero} : {ted.Intitule}";

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
                            t.Lane_Id = BoardESrc.ID_SUIVI.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_SUIVI.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_TACHE.ToString();
                            break;
                        case DomaineTED.Joachim:
                            t.Lane_Id = BoardJOH1.ID_SUIVI.ToString();
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
                        case DomaineTED.Joachim:
                            t.Lane_Id = BoardJOH1.ID_EVOLUTION.ToString();
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
                        case DomaineTED.Joachim:
                            t.Lane_Id = BoardJOH1.ID_TACHE.ToString();
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
                        else if (ted.VersionPrevue.Contains("4.23"))
                        {
                            t.Card_Type = Card.TYPE_VINGTTROIS;
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
                        case DomaineTED.Joachim:
                            t.Lane_Id = BoardJOH1.ID_IMPREVU.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
                default:
                    t.Card_Type = "INCONNU";
                    switch (domaine)
                    {
                        case DomaineTED.eSRC:
                            t.Lane_Id = BoardESrc.ID_DEFAUT.ToString();
                            break;
                        case DomaineTED.Tauri:
                            t.Lane_Id = BoardTauri.ID_DEFAUT.ToString();
                            break;
                        case DomaineTED.FSS:
                            t.Lane_Id = BoardFSS.ID_DEFAUT.ToString();
                            break;
                        case DomaineTED.Joachim:
                            t.Lane_Id = BoardJOH1.ID_DEFAUT.ToString();
                            break;
                        default:
                            t.Lane_Id = "0";
                            break;
                    }
                    break;
            }
            //traitement specifique des cartes sur Joachim
            if (domaine == DomaineTED.Joachim)
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
                else if (ted.VersionPrevue.Contains("4.23"))
                {
                    t.Card_Type = Card.TYPE_VINGTTROIS;
                }
                else
                {
                    t.Card_Type = ted.VersionPrevue;
                }
            }
            return t;
        }


        public static DomaineTED GetDomaine(ExportTED ted)
        {
            if (ted.SousSysteme == "Prestations FSS")
            {
                return DomaineTED.FSS;
            }
            else if (ted.SousSysteme == "eSRC" || ted.Intitule.ToUpper().StartsWith("ESRC"))
            {
                return DomaineTED.eSRC;
            }
            else if (ted.SousSysteme == "ServicesCommuns")
            {
                return DomaineTED.Commun;
            }
            else if (ted.SousSysteme.StartsWith("Actuaria"))
            {
                return DomaineTED.Tauri;
            }
            else if (ted.SousSysteme == "Joachim")
            {
                return DomaineTED.Joachim;
            } else
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
        Inconnu,
        Joachim
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
