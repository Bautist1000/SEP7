namespace AquAnalyzerWebApp.Models
{
    public class WaterDataDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double UsageVolume { get; set; }
        public double FlowRate { get; set; }
        public double ElectricityConsumption { get; set; }
        public double ProductId { get; set; }
        public string SourceType { get; set; }
        public bool LeakDetected { get; set; }
        public string Location { get; set; }
        public bool HasAbnormalities { get; set; }
        public bool UsesCleanEnergy { get; set; }
    }

}