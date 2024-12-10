using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class WaterMetricsService : IWaterMetricsService
    {
        private readonly DatabaseContext context;

        public WaterMetricsService(DatabaseContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync()
        {
            return await context.WaterMetrics.ToListAsync();
        }

        public async Task<WaterMetrics> GetMetricsByIdAsync(int id)
        {
            return await context.WaterMetrics.FindAsync(id);
        }
        // generate metrics for each location from raw data
        public async Task GenerateMetricsAsync(IEnumerable<WaterData> data)
        {
            var groupedData = data.GroupBy(d => d.Location);
            foreach (var group in groupedData)
            {
                var metrics = new WaterMetrics
                {
                    DateGeneratedOn = DateTime.Now,
                    LeakageRate = group.Average(d => d.LeakDetected == true ? 1 : 0),
                    WaterEfficiencyRatio = group.Average(d => d.UsageVolume / d.ElectricityConsumption),
                    TotalWaterConsumption = group.Sum(d => d.UsageVolume),
                    TotalWaterSaved = group.Where(d => d.SourceType == "recycled").Sum(d => d.UsageVolume),
                    RecycledWaterUsage = group.Where(d => d.SourceType == "recycled").Sum(d => d.UsageVolume),
                };
                await context.WaterMetrics.AddAsync(metrics);
            }
            await context.SaveChangesAsync();
        }

        public async Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData)
        {
            return WaterMetrics.CalculateAverageFlowRate(waterData);
        }

        public async Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData)
        {
            return WaterMetrics.CountAbnormalities(waterData);
        }

        public async Task AddMetricsAsync(WaterMetrics metrics)
        {
            await context.WaterMetrics.AddAsync(metrics);
            await context.SaveChangesAsync();
        }

        public async Task UpdateMetricsAsync(WaterMetrics metrics)
        {
            context.WaterMetrics.Update(metrics);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMetricsAsync(int id)
        {
            var metrics = await context.WaterMetrics.FindAsync(id);
            if (metrics != null)
            {
                context.WaterMetrics.Remove(metrics);
                await context.SaveChangesAsync();
            }
        }
    }
}