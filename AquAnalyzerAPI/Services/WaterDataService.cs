using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WaterDataService : IWaterDataService
{
    private readonly DatabaseContext context;

    public WaterDataService(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<WaterData> GetByIdAsync(int id)
    {
        return await context.WaterData.FindAsync(id);
    }

    public async Task<IEnumerable<WaterData>> GetAllAsync()
    {
        return await context.WaterData.ToListAsync();
    }

    public async Task AddWaterDataAsync(WaterData data)
    {
        await context.WaterData.AddAsync(data);
        await context.SaveChangesAsync();
    }

    public async Task UpdateWaterDataAsync(WaterData data)
    {
        context.WaterData.Update(data);
        await context.SaveChangesAsync();
    }

    public async Task DeleteWaterDataAsync(int id)
    {
        var data = await GetByIdAsync(id);
        if (data != null)
        {
            context.WaterData.Remove(data);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<WaterData>> GetByLocationAsync(string location)
    {
        return await context.WaterData
            .Where(d => d.Location == location)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetWithAbnormalitiesAsync()
    {
        return await context.WaterData
            .Where(d => d.HasAbnormalities)
            .ToListAsync();
    }

    public async Task GenerateMetricsAsync(IEnumerable<WaterData> waterData)
    {
        var groupedData = waterData.GroupBy(wd => wd.Location);
        foreach (var group in groupedData)
        {
            var metrics = new WaterMetrics
            {
                DateGeneratedOn = DateTime.Now
            };
            metrics.CalculateMetrics(group);

            await context.WaterMetrics.AddAsync(metrics);
        }
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<WaterData>> GetBySourceTypeAsync(string sourceType)
    {
        return await context.WaterData
            .Where(d => d.SourceType == sourceType)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByUsageVolumeRangeAsync(double minVolume, double maxVolume)
    {
        return await context.WaterData
            .Where(d => d.UsageVolume >= minVolume && d.UsageVolume <= maxVolume)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByFlowRateRangeAsync(double minFlowRate, double maxFlowRate)
    {
        return await context.WaterData
            .Where(d => d.FlowRate >= minFlowRate && d.FlowRate <= maxFlowRate)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByElectricityConsumptionRangeAsync(double minConsumption, double maxConsumption)
    {
        return await context.WaterData
            .Where(d => d.ElectricityConsumption >= minConsumption && d.ElectricityConsumption <= maxConsumption)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByProductIdAsync(double productId)
    {
        return await context.WaterData
            .Where(d => d.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByLeakDetectedAsync(bool leakDetected)
    {
        return await context.WaterData
            .Where(d => d.LeakDetected == leakDetected)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterData>> GetByCleanEnergyUsageAsync(bool usesCleanEnergy)
    {
        return await context.WaterData
            .Where(d => d.UsesCleanEnergy == usesCleanEnergy)
            .ToListAsync();
    }
}