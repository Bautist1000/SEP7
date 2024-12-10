namespace AquAnalyzerAPI.Models
{
    public class Visualisation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<WaterMetrics> MetricsUsed = [];
        public List<WaterData> RawDataUsed = [];

    }
}