using CsvHelper.Configuration;
using TEDExporter.DTO;

namespace TEDExporter
{
    public class CSvMapperInit : CsvClassMap<ExportInit>
    {

        public CSvMapperInit()
        {
            Map(m => m.Numero).Name("Numero");
        }
    }
}
