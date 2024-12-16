using System;
using System.Collections.Generic;

namespace AquAnalyzerAPI.Models
{
    public class Visualisation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ReportId { get; set; }
        public List<WaterMetrics> MetricsUsed = [];
        public List<WaterData> RawDataUsed = [];
        public Report Report { get; set; }

        public Visualisation(int Id, string Type, int ReportId)
        {
            this.Id = Id;
            this.Type = Type;
            this.ReportId = ReportId;
        }

    }
}