namespace AquAnalyzerWebApp.Models
{
    public class WaterMetricsDto
    {
        public int Id { get; set; }
        public DateTime DateGeneratedOn { get; set; }
        public double LeakageRate { get; set; }
        public double WaterEfficiencyRatio { get; set; }
        public double TotalWaterConsumption { get; set; }
        public double TotalWaterSaved { get; set; }
        public double RecycledWaterUsage { get; set; }
    }
}