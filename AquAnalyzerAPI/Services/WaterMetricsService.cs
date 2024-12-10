using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class WaterMetricsService : IWaterMetricsService
{
    private readonly DatabaseContext context;

    public WaterMetricsService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<WaterMetrics> GetByIdAsync(int id)
    {
        await context.WaterMetrics.FindAsync(id);
    }

    public async Task<IEnumerable<WaterMetrics>> GetAllAsync()
    {
        await context.WaterMetrics.ToListAsync();
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
