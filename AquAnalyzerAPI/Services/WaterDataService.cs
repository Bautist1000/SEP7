using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;
using Microsoft.Extensions.Logging;

namespace AquAnalyzerAPI.Services
{
    public class WaterDataService : IWaterDataService
    {
        private readonly DatabaseContext context;
        private readonly IAbnormalityService _abnormalityService;
        private readonly ILogger<WaterDataService> _logger;

        public WaterDataService(DatabaseContext context, IAbnormalityService abnormalityService, ILogger<WaterDataService> logger)
        {
            this.context = context;
            _abnormalityService = abnormalityService;
            _logger = logger;
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
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Adding water data.");

                // Remove Id assignment - let database generate it
                data.Id = 0;

                await context.WaterData.AddAsync(data);
                await context.SaveChangesAsync();

                var abnormalities = await _abnormalityService?.CheckWaterDataAbnormalities(data.Id);

                if (abnormalities != null && abnormalities.Any())
                {
                    data.HasAbnormalities = true;
                    context.Entry(data).State = EntityState.Modified;

                    foreach (var abnormality in abnormalities)
                    {
                        await context.Abnormalities.AddAsync(abnormality);
                    }

                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add water data.");
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateWaterDataAsync(WaterData data)
        {
            try
            {
                _logger.LogInformation($"Updating water data with ID: {data.Id}");

                // Attach the main entity
                context.Entry(data).State = EntityState.Modified;

                // Only update the foreign key
                context.Entry(data).Property(d => d.WaterMetricsId).IsModified = true;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update water data.");
                throw;
            }
        }

        public async Task DeleteWaterDataAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting water data with ID: {id}");
                var data = await GetWaterDataByIdAsync(id);

                if (data != null)
                {
                    context.WaterData.Remove(data);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete water data.");
                throw;
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
            try
            {
                _logger.LogInformation("Generating water metrics.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate metrics.");
                throw;
            }
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
