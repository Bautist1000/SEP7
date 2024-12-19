namespace AquAnalyzerAPI.Models
{
    public class VisualisationData
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public List<WaterMetrics> MetricsUsed { get; set; } = new List<WaterMetrics>();
        public List<WaterData> RawDataUsed { get; set; } = new List<WaterData>();
        public ChartConfiguration ChartConfig { get; set; }

        public VisualisationData()
        {
            MetricsUsed = new List<WaterMetrics>();
            RawDataUsed = new List<WaterData>();
            ChartConfig = new ChartConfiguration();
        }

        public VisualisationData(int id, string type, int reportId)
        {
            Id = id;
            Type = type;
            ReportId = reportId;
            MetricsUsed = new List<WaterMetrics>();
            RawDataUsed = new List<WaterData>();
            ChartConfig = new ChartConfiguration();
        }
    }
}