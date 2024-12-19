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
    public class AbnormalityService : IAbnormalityService
    {
        private readonly DatabaseContext _context;
        private readonly INotificationService _notificationService;
        private const double MAX_ALLOWED_FLOW_DURATION = 100.0; // Change to sensible values for Kamstrup.
        private const double MAX_ELECTRICITY_CONSUMPTION = 1000.0; // Change to sensible values for Kamstrup.
        private const double MIN_WATER_EFFICIENCY = 0.5; // Change to sensible values for Kamstrup.
        private const double MAX_LEAKAGE_RATE = 10.0; // Change to sensible values for Kamstrup.
        private const double MIN_USAGE_DURATION = 0.5; // Change to sensible values for Kamstrup.
        private const double ELECTRICITY_RATE_PER_FLOW = 0.1; // Change to sensible values for Kamstrup.
        private const double ELECTRICITY_TOLERANCE = 0.1; // Change to sensible values for Kamstrup.

        public AbnormalityService(DatabaseContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Abnormality> AddAbnormality(Abnormality abnormality)
        {
            // First get potential matches by WaterDataId and Type(database side)
            var potentialMatches = await _context.Abnormalities
                .Where(a => a.WaterDataId == abnormality.WaterDataId &&
                            a.Type == abnormality.Type)
                .ToListAsync();

            // Then check timestamp and description in memory
            var existingAbnormality = potentialMatches
                .FirstOrDefault(a =>
                    a.Description == abnormality.Description &&
                    Math.Abs((a.Timestamp - abnormality.Timestamp).TotalSeconds) < 1);

            if (existingAbnormality != null)
            {
                await _notificationService.CreateNotificationFromAbnormality(existingAbnormality);
                return existingAbnormality;
            }

            await _context.Abnormalities.AddAsync(abnormality);
            await _context.SaveChangesAsync();

            // Create notification after saving abnormality
            await _notificationService.CreateNotificationFromAbnormality(abnormality);

            return abnormality;
        }

        public async Task<IEnumerable<Abnormality>> GetAllAbnormalities()
        {
            return await _context.Abnormalities.ToListAsync();
        }

        public async Task<Abnormality> GetAbnormalityById(int id)
        {
            return await _context.Abnormalities.FindAsync(id);
        }

        public async Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type)
        {
            return await _context.Abnormalities
                .Where(a => a.Type == type)
                .ToListAsync();
        }

        public async Task<bool> MarkAbnormalityAsDealtWith(int abnormalityId)
        {

            var abnormality = await _context.Abnormalities.FindAsync(abnormalityId);
            if (abnormality != null)
            {
                abnormality.IsDealtWith = true;

                // Update related water data if no other active abnormalities exist
                var waterData = await _context.WaterData
                    .Include(w => w.Abnormalities)
                    .FirstOrDefaultAsync(w => w.Id == abnormality.WaterDataId);

                if (waterData != null)
                {
                    var hasActiveAbnormalities = await _context.Abnormalities
                        .AnyAsync(a => a.WaterDataId == waterData.Id && !a.IsDealtWith);

                    waterData.HasAbnormalities = hasActiveAbnormalities;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateAbnormality(int id, string description, string type)
        {
            var abnormality = await _context.Abnormalities.FindAsync(id);
            if (abnormality == null)
            {
                return false;
            }

            abnormality.Description = description;
            abnormality.Type = type;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAbnormality(int id)
        {
            var abnormality = await _context.Abnormalities.FindAsync(id);
            if (abnormality == null)
            {
                return false;
            }

            _context.Abnormalities.Remove(abnormality);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveDuplicateAbnormalities(int waterDataId)
        {
            var existingAbnormalities = await _context.Abnormalities
         .Where(a => a.WaterDataId == waterDataId)
         .ToListAsync();

            var groupedAbnormalities = existingAbnormalities
                .GroupBy(a => new
                {
                    // Round to nearest second by flooring ticks
                    Timestamp = DateTime.SpecifyKind(
                        new DateTime(a.Timestamp.Ticks - (a.Timestamp.Ticks % TimeSpan.TicksPerSecond),
                        DateTimeKind.Utc),
                        DateTimeKind.Utc),
                    a.WaterDataId,
                    a.Type
                })
                .Where(g => g.Count() > 1)
                .ToList(); // Materialize the query

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var group in groupedAbnormalities)
                {
                    var duplicates = group.OrderBy(a => a.Id).Skip(1).ToList(); // Keep oldest, remove others
                    foreach (var duplicate in duplicates)
                    {
                        var notifications = await _context.Notifications
                            .Where(n => n.AbnormalityId == duplicate.Id)
                            .ToListAsync();

                        _context.Notifications.RemoveRange(notifications);
                        _context.Abnormalities.Remove(duplicate);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId)
        {
            var abnormalities = new List<Abnormality>();
            var waterData = await _context.WaterData
                .FirstOrDefaultAsync(w => w.Id == dataId);

            if (waterData == null) return abnormalities;

            var existingAbnormalities = await _context.Abnormalities
      .Where(a => a.WaterDataId == dataId)
      .ToListAsync();

            var groupedAbnormalities = existingAbnormalities
                .GroupBy(a => new
                {
                    Timestamp = DateTime.SpecifyKind(
                        new DateTime(a.Timestamp.Ticks - (a.Timestamp.Ticks % TimeSpan.TicksPerSecond),
                        DateTimeKind.Utc),
                        DateTimeKind.Utc),
                    a.WaterDataId,
                    a.Type
                })
                .Where(g => g.Count() > 1);

            foreach (var group in groupedAbnormalities)
            {
                var duplicates = group.OrderBy(a => a.Id).Skip(1).ToList();
                foreach (var duplicate in duplicates)
                {
                    var notifications = await _context.Notifications
                        .Where(n => n.AbnormalityId == duplicate.Id)
                        .ToListAsync();

                    _context.Notifications.RemoveRange(notifications);
                    _context.Abnormalities.Remove(duplicate);
                }
            }

            var existingTypes = await _context.Abnormalities
                .Where(a => a.WaterDataId == dataId)
                .Select(a => new { a.Type, a.Description })
                .ToListAsync();

            var processedTypes = new HashSet<(string type, string desc)>();

            // Check Timestamp
            if (waterData.Timestamp == default(DateTime))
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Missing Timestamp",
                    Description = "Timestamp is missing or null.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            else if (waterData.Timestamp > DateTime.Now)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Future Timestamp",
                    Description = "Timestamp is set in the future.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check UsageVolume
            if (waterData.UsageVolume <= 0)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Usage Volume",
                    Description = "Usage volume is negative or zero.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Fetch historical data for the same location
            var historicalData = await _context.WaterData
                .Where(w => w.Location == waterData.Location && w.Timestamp < waterData.Timestamp)
                .OrderByDescending(w => w.Timestamp)
                .Take(30)
                .ToListAsync();

            if (historicalData.Count > 10)
            {
                double averageUsage = historicalData.Average(h => h.UsageVolume);
                double standardDeviation = Math.Sqrt(historicalData.Average(h => Math.Pow(h.UsageVolume - averageUsage, 2)));
                // Check for significant deviations from historical average
                if (Math.Abs(waterData.UsageVolume - averageUsage) > 2 * standardDeviation)
                {
                    abnormalities.Add(new Abnormality
                    {
                        Type = "Usage Volume Anomaly",
                        Description = $"Usage volume deviates significantly from historical average: {averageUsage:F2} (±{standardDeviation:F2}).",
                        Timestamp = DateTime.Now,
                        WaterDataId = waterData.Id,
                        WaterData = waterData
                    });
                }

                // Check for a sudden spike in usage compared to recent records
                double maxRecentUsage = historicalData.Max(h => h.UsageVolume);
                if (waterData.UsageVolume > maxRecentUsage * 1.5) // 50% higher than the maximum recent usage
                {
                    abnormalities.Add(new Abnormality
                    {
                        Type = "Sudden Spike in Usage",
                        Description = $"Usage volume is unusually high compared to recent maximum: {maxRecentUsage:F2}.",
                        Timestamp = DateTime.Now,
                        WaterDataId = waterData.Id,
                        WaterData = waterData
                    });
                }

                // Seasonal Trends: Compare usage for the same period in previous years
                var seasonalData = await _context.WaterData
                    .Where(w => w.Location == waterData.Location &&
                                w.Timestamp.Month == waterData.Timestamp.Month &&
                                w.Timestamp.Day == waterData.Timestamp.Day &&
                                w.Timestamp.Year < waterData.Timestamp.Year)
                    .ToListAsync();

                if (seasonalData.Any())
                {
                    double seasonalAverage = seasonalData.Average(s => s.UsageVolume);
                    if (Math.Abs(waterData.UsageVolume - seasonalAverage) > 2 * standardDeviation)
                    {
                        abnormalities.Add(new Abnormality
                        {
                            Type = "Seasonal Usage Anomaly",
                            Description = $"Usage volume deviates significantly from seasonal average: {seasonalAverage:F2}.",
                            Timestamp = DateTime.Now,
                            WaterDataId = waterData.Id,
                            WaterData = waterData
                        });
                    }
                }

                // Time-of-Day Patterns: Use hourly averages to detect anomalies
                var hourlyData = await _context.WaterData
                    .Where(w => w.Location == waterData.Location &&
                                w.Timestamp.Hour == waterData.Timestamp.Hour)
                    .ToListAsync();

                if (hourlyData.Any())
                {
                    double hourlyAverage = hourlyData.Average(h => h.UsageVolume);
                    if (Math.Abs(waterData.UsageVolume - hourlyAverage) > 2 * standardDeviation)
                    {
                        abnormalities.Add(new Abnormality
                        {
                            Type = "Hourly Usage Anomaly",
                            Description = $"Usage volume deviates significantly from hourly average: {hourlyAverage:F2}.",
                            Timestamp = DateTime.Now,
                            WaterDataId = waterData.Id,
                            WaterData = waterData
                        });
                    }
                }

                // Rolling Averages: Compute a 7-day rolling average for smoother trend analysis
                var rollingData = await _context.WaterData
                    .Where(w => w.Location == waterData.Location &&
                                w.Timestamp >= waterData.Timestamp.AddDays(-7) &&
                                w.Timestamp < waterData.Timestamp)
                    .ToListAsync();

                if (rollingData.Any())
                {
                    double rollingAverage = rollingData.Average(r => r.UsageVolume);
                    if (Math.Abs(waterData.UsageVolume - rollingAverage) > 2 * standardDeviation)
                    {
                        abnormalities.Add(new Abnormality
                        {
                            Type = "Rolling Average Anomaly",
                            Description = $"Usage volume deviates significantly from 7-day rolling average: {rollingAverage:F2}.",
                            Timestamp = DateTime.Now,
                            WaterDataId = waterData.Id,
                            WaterData = waterData
                        });
                    }
                }
            }
            else
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Insufficient Historical Data",
                    Description = "Not enough historical data to perform a reliable analysis.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check FlowRate
            if (waterData.FlowRate <= 0)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Flow Rate",
                    Description = "Flow rate is negative or zero.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Consistency between FlowRate and UsageVolume
            if (waterData.FlowRate > 0 && waterData.UsageVolume / waterData.FlowRate > MAX_ALLOWED_FLOW_DURATION)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Inconsistent Flow Rate",
                    Description = "Flow rate is inconsistent with usage volume.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Consistency between FlowRate and UsageVolume
            if (waterData.FlowRate > 0 && waterData.UsageVolume / waterData.FlowRate < MIN_USAGE_DURATION)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Inconsistent Flow Rate and Volume",
                    Description = "Flow rate and usage volume suggest an illogical duration.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Check ElectricityConsumption
            if (waterData.ElectricityConsumption <= 0)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Electricity Consumption",
                    Description = "Electricity consumption is negative or zero.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Check for excessive electricity consumption
            if (waterData.ElectricityConsumption > MAX_ELECTRICITY_CONSUMPTION)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "High Electricity Consumption",
                    Description = "Electricity consumption is abnormally high.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Check for electricity and flow rate mismatch
            double expectedElectricity = waterData.FlowRate * ELECTRICITY_RATE_PER_FLOW;
            if (Math.Abs(waterData.ElectricityConsumption - expectedElectricity) > ELECTRICITY_TOLERANCE)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Electricity and Flow Mismatch",
                    Description = $"Electricity consumption deviates from expected value: {expectedElectricity:F2}.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check ProductId
            if (waterData.ProductId <= 0)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Product ID",
                    Description = "Product ID is missing or negative.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check SourceType
            var validSourceTypes = new[] { "freshwater", "recycled", "groundwater", "seawater" };
            if (string.IsNullOrEmpty(waterData.SourceType) || !validSourceTypes.Contains(waterData.SourceType))
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Source Type",
                    Description = "Source type is missing or invalid.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check Location
            if (string.IsNullOrEmpty(waterData.Location))
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Missing Location",
                    Description = "Location is missing or null.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            // Check LeakDetected
            if (waterData.LeakDetected == null)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Leak Detection Unavailable",
                    Description = "LeakDetected field is null.",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }
            return abnormalities;

        }


        public async Task<IEnumerable<Abnormality>> CheckWaterMetricsAbnormalities(int metricsId)
        {
            var abnormalities = new List<Abnormality>();
            var metrics = await _context.WaterMetrics
               .FirstOrDefaultAsync(m => m.Id == metricsId);

            if (metrics == null)
                return abnormalities;

            // Check for unusual water efficiency ratio
            if (metrics.WaterEfficiencyRatio <= 0 || metrics.WaterEfficiencyRatio > 1)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Water Efficiency Ratio",
                    Description = $"Water efficiency ratio is outside the logical range: {metrics.WaterEfficiencyRatio:F2}",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            // Check for high leakage rate
            if (metrics.LeakageRate > MAX_LEAKAGE_RATE)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "High Leakage Rate",
                    Description = $"Leakage rate is above threshold: {metrics.LeakageRate:F2}%",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            // Check TotalWaterConsumption for unusual values
            if (metrics.TotalWaterConsumption <= 0)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Total Water Consumption",
                    Description = "Total water consumption is zero or negative, which is illogical.",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            // Check TotalWaterSaved exceeds TotalWaterConsumption
            if (metrics.TotalWaterSaved > metrics.TotalWaterConsumption)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Invalid Water Savings",
                    Description = $"Total water saved ({metrics.TotalWaterSaved:F2}) exceeds total water consumption ({metrics.TotalWaterConsumption:F2}).",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            // Check RecycledWaterUsage is within expected norms
            double recycledPercentage = metrics.RecycledWaterUsage / metrics.TotalWaterConsumption * 100;
            if (recycledPercentage < 10 || recycledPercentage > 50)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Unusual Recycled Water Usage",
                    Description = $"Recycled water usage is {recycledPercentage:F2}% of total, outside expected range (10%-50%).",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            // Validate DateGeneratedOn
            if (metrics.DateGeneratedOn > DateTime.Now)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Future Date",
                    Description = "DateGeneratedOn is set in the future, which is not valid.",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }
            else if ((DateTime.Now - metrics.DateGeneratedOn).TotalDays > 365)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Outdated Metrics",
                    Description = "DateGeneratedOn is older than a year, suggesting outdated metrics.",
                    Timestamp = DateTime.Now,
                    WaterMetricsId = metrics.Id,
                    WaterMetrics = metrics
                });
            }

            return abnormalities;
        }

    }
}



