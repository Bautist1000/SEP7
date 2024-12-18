using System;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Models
{
    public class WaterData
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double UsageVolume { get; set; }
        public double FlowRate { get; set; }
        public double ElectricityConsumption { get; set; }
        public double ProductId { get; set; }
        public string SourceType { get; set; }
        public bool LeakDetected { get; set; } = false;
        public string Location { get; set; }
        public bool HasAbnormalities { get; set; } = false;
        public bool UsesCleanEnergy { get; set; } = false;

        public int? WaterMetricsId { get; set; }
        public WaterMetrics? WaterMetrics { get; set; }

        public Abnormality? Abnormality { get; set; }

        public List<VisualisationData> Visualisations { get; set; } = new List<VisualisationData>();
        public WaterData() { }
        public WaterData(int Id, DateTime Timestamp)
        {
            this.Id = Id;
            this.Timestamp = Timestamp;
        }
        public WaterData(int Id, DateTime Timestamp, double UsageVolume, double FlowRate, double ElectricityConsumption, double ProductId, string SourceType, bool LeakDetected, string Location, bool HasAbnormalities, bool UsesCleanEnergy)
        {
            this.Id = Id;
            this.Timestamp = Timestamp;
            this.UsageVolume = UsageVolume;
            this.FlowRate = FlowRate;
            this.ElectricityConsumption = ElectricityConsumption;
            this.ProductId = ProductId;
            this.SourceType = SourceType;
            this.LeakDetected = LeakDetected;
            this.Location = Location;
            this.HasAbnormalities = HasAbnormalities;
            this.UsesCleanEnergy = UsesCleanEnergy;
        }
    }

}