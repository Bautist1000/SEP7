using AquAnalyzerAPI.Models;
namespace AquAnalyzerAPI.Dtos
{
    public class VisualisationDataDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ChartConfiguration ChartConfig { get; set; }
        public ICollection<WaterMetricsDto> MetricsUsed { get; set; }
        public ICollection<WaterDataDto> RawDataUsed { get; set; }
    }

}