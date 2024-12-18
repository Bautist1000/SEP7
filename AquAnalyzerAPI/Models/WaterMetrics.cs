using System;
using System.Collections.Generic;
using System.Linq;

namespace AquAnalyzerAPI.Models
{
    public class WaterMetrics
    {
        public int Id { get; set; }
        public DateTime DateGeneratedOn { get; set; }
        public double LeakageRate { get; set; }
        public double WaterEfficiencyRatio { get; set; }
        public double TotalWaterConsumption { get; set; }
        public double TotalWaterSaved { get; set; }
        public double RecycledWaterUsage { get; set; }

        public List<WaterData> WaterData { get; set; } = new List<WaterData>();

        public Abnormality? Abnormality { get; set; }

        public List<VisualisationData> Visualisations { get; set; } = new List<VisualisationData>();

        public WaterMetrics()
        {

        }
        public WaterMetrics(int Id, DateTime DateGeneratedOn)
        {
            this.Id = Id;
            this.DateGeneratedOn = DateGeneratedOn;
        }

        public WaterMetrics(int Id, DateTime DateGeneratedOn, double LeakageRate, double WaterEfficiencyRatio, double TotalWaterConsumption, double TotalWaterSaved, double RecycledWaterUsage)
        {
            this.Id = Id;
            this.DateGeneratedOn = DateGeneratedOn;
            this.LeakageRate = LeakageRate;
            this.WaterEfficiencyRatio = WaterEfficiencyRatio;
            this.TotalWaterConsumption = TotalWaterConsumption;
            this.TotalWaterSaved = TotalWaterSaved;
            this.RecycledWaterUsage = RecycledWaterUsage;
        }
        public void CalculateMetrics(IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                throw new ArgumentException("WaterData cannot be null or empty");

            LeakageRate = waterData.Count(wd => wd.LeakDetected == true) / (double)waterData.Count() * 100;

            WaterEfficiencyRatio = waterData
                .Where(wd => wd.ElectricityConsumption > 0)
                .Average(wd => wd.UsageVolume / wd.ElectricityConsumption);

            TotalWaterConsumption = waterData.Sum(wd => wd.UsageVolume);

            TotalWaterSaved = waterData
                .Where(wd => wd.SourceType.Equals("recycled", StringComparison.OrdinalIgnoreCase))
                .Sum(wd => wd.UsageVolume);

            RecycledWaterUsage = TotalWaterSaved;
        }

        public static double CalculateAverageFlowRate(IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                throw new ArgumentException("WaterData cannot be null or empty");

            return waterData.Average(wd => wd.FlowRate);
        }

        public static int CountAbnormalities(IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                throw new ArgumentException("WaterData cannot be null or empty");

            return waterData.Count(wd => wd.HasAbnormalities);
        }

    }
}
