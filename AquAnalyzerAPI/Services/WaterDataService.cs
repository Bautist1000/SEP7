using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;
namespace AquAnalyzerAPI.Services
{

    public class WaterDataService : IWaterDataService
    {
        private readonly DatabaseContext context;

        public WaterDataService(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<WaterData> GetWaterDataByIdAsync(int id)
        {
            return await context.WaterData.FindAsync(id);
        }

        public async Task<IEnumerable<WaterData>> GetAllWaterDataAsync()
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
            if (data == null) throw new ArgumentNullException(nameof(data));

            // Attach the main entity
            context.Entry(data).State = EntityState.Modified;

            // Only update the foreign key (no need to attach the WaterMetrics object)
            context.Entry(data).Property(d => d.WaterMetricsId).IsModified = true;

            // Save changes
            await context.SaveChangesAsync();
        }



        public async Task DeleteWaterDataAsync(int id)
        {
            var data = await GetWaterDataByIdAsync(id);
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
}
