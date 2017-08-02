using CsvHelper.Configuration;
using TEDExporter.DTO;

namespace TEDExporter
{
    public class CSvMapperTED : CsvClassMap<ExportTED>
    {

        public CSvMapperTED()
        {
            Map(m => m.Numero).Name("Numéro");
            Map(m => m.Type).Name("Type");
            Map(m => m.SousType).Name("Sous-type");
            Map(m => m.Statut).Name("Statut");
            Map(m => m.Priorite).Name("Priorité");
            Map(m => m.Domaine).Index(5);
            Map(m => m.SousSysteme).Name("Sous-système");
            Map(m => m.Organisme).Name("Organisme");
            Map(m => m.Nature).Name("Nature");
            Map(m => m.Severite).Name("Sévérité");
            Map(m => m.AffecteA).Name("Affecté à");
            Map(m => m.Emetteur).Name("Emetteur");
            Map(m => m.ServiceCreat).Name("Service creat.");
            Map(m => m.VersionPrevue).Name("Version prévue");
            Map(m => m.HistoAffect).Name("Histo.Affec.");
            Map(m => m.Intitule).Name("Intitulé");
            Map(m => m.Environnement).Name("Env.");


        }
    }
}
