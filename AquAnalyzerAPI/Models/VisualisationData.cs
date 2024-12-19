using Newtonsoft.Json;
namespace AquAnalyzerAPI.Models
{
    public class VisualisationData
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ReportId { get; set; }
        [JsonIgnore]
        public Report Report { get; set; }
        public virtual ChartConfiguration ChartConfig { get; set; }

        public virtual ICollection<WaterMetrics> MetricsUsed { get; set; } = new List<WaterMetrics>();
        public virtual ICollection<WaterData> RawDataUsed { get; set; } = new List<WaterData>();

        public VisualisationData()
        {
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