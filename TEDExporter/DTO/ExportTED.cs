using System;

namespace TEDExporter.DTO
{
    public class ExportTED
    {
        public int Numero { get; set; }
        public string Type { get; set; }
        public string SousType { get; set; }
        public string Statut { get; set; }
        public string Priorite { get; set; }
        public string Domaine { get; set; }
        public string SousSysteme { get; set; }
        public string Organisme { get; set; }
        public string Nature { get; set; }
        public string Severite { get; set; }
        public string AffecteA { get; set; }
        public string DateEmission { get; set; }
        public string Emetteur { get; set; }
        public string ServiceCreat { get; set; }
        public string VersionPrevue { get; set; }
        public string HistoAffect { get; set; }
        public string Intitule { get; set; }
        public string Environnement { get; set; }

    }
}
