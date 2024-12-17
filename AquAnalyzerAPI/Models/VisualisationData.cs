namespace AquAnalyzerAPI.Models
{
    public class VisualisationData
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ReportId { get; set; }
        public List<WaterMetrics> MetricsUsed { get; set; } = new List<WaterMetrics>();
        public List<WaterData> RawDataUsed { get; set; } = new List<WaterData>();
        public Report Report { get; set; }

        public VisualisationData(int id, string type, int reportId)
        {
            Id = id;
            Type = type;
            ReportId = reportId;
        }

        public VisualisationData()
        {
            
        }

}
}