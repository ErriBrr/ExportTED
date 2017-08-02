using TEDExporter.DTO;

namespace TEDExporter
{
    public static class TedToLeaKitMapper
    {

        public static ImportLeanKit GetDTOForTED(ExportTED ted)
        {
            var t = new ImportLeanKit();
            t.Card_Description = ted.Intitule;
            t.Card_Title = "TED " + ted.Numero;
            var domaine = GetDomaine(ted);
            var nature = GetNature(ted);
            switch (nature)
            {
                case "SuiviProd":
                    t.Lane_Id = "521748899";
                    t.Card_Type = "519685034";
                    break;
                case "Evol":
                    t.Card_Type = "512063321";
                    switch (domaine)
                    {
                        case "FSS":
                            t.Lane_Id = "516686010";
                            break;
                        case "eSRC":
                            t.Lane_Id = "521517640";
                            break;
                        case "Tauri":
                            t.Lane_Id = "516686007";
                            break;
                    }
                    break;
                case "Tache":
                    t.Card_Type = "512063320";
                    switch (domaine)
                    {
                        case "FSS":
                            t.Lane_Id = "512720055";
                            break;
                        case "eSRC":
                            t.Lane_Id = "512726405";
                            break;
                        case "Tauri":
                            t.Lane_Id = "512720054";
                            break;
                    }
                    break;
                case "Imprevu":
                    t.Card_Type = "512063321";
                    switch (domaine)
                    {
                        case "FSS":
                            t.Lane_Id = "512720057";
                            break;
                        case "eSRC":
                            t.Lane_Id = "512726407";
                            break;
                        case "Tauri":
                            t.Lane_Id = "512720056";
                            break;
                    }
                    break;

            }
            return t;
        
        }


        public static string GetDomaine(ExportTED ted)
        {
            if (ted.SousSysteme == "Prestations FSS")
            {
                return "FSS";
            }
            if(ted.SousSysteme == "eSRC")
            {
                return "eSRC";
            }
            if(ted.SousSysteme == "ServicesCommuns")
            {
                return "commun";
            }
            if (ted.SousSysteme.StartsWith("Actuaria"))
            {
                return "Tauri";
            }
            return string.Empty;

        }

        public static string GetNature(ExportTED ted)
        {
            if (ted.Intitule.StartsWith("[Suivi"))
            {
                return "SuiviProd";
            }
            if(ted.Type == "Evolution")
            {
                return "Evol";
            }

            if(ted.Type == "Imprévu")
            {
                return "Imprevu";
            }

            if(ted.Type == "Tâche")
            {
                return "Tache";
            }
            return string.Empty;
        }
    }
}
