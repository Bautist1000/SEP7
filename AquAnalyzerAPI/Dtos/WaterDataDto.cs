using System.ComponentModel.DataAnnotations;
namespace AquAnalyzerAPI.Dtos
{
    public class WaterDataDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double UsageVolume { get; set; }
        public double FlowRate { get; set; }
        public double ElectricityConsumption { get; set; }
        public double ProductId { get; set; }

        [Required(ErrorMessage = "Source Type is required")]
        public string SourceType { get; set; }

        public bool LeakDetected { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        public bool HasAbnormalities { get; set; }
        public bool UsesCleanEnergy { get; set; }
    }
}