using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class AbnormalityService : IAbnormalityService
    {
        private readonly DatabaseContext _context;
        private const double MAX_FLOW_RATE = 100.0;
        private const double MAX_ELECTRICITY_CONSUMPTION = 1000.0;
        private const double MIN_WATER_EFFICIENCY = 0.5;
        private const double MAX_LEAKAGE_RATE = 10.0;

        public AbnormalityService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Abnormality> AddAbnormality(Abnormality abnormality)
        {
            await _context.Abnormalities.AddAsync(abnormality);
            await _context.SaveChangesAsync();
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

           public async Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId)
        {
            var abnormalities = new List<Abnormality>();
            var waterData = await _context.WaterData
                .FirstOrDefaultAsync(w => w.Id == dataId);

            if (waterData == null)
                return abnormalities;

            // Check for unusual flow rate
            if (waterData.FlowRate > MAX_FLOW_RATE)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "High Flow Rate",
                    Description = $"Flow rate is unusually high: {waterData.FlowRate:F2}",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check for high electricity consumption
            if (waterData.ElectricityConsumption > MAX_ELECTRICITY_CONSUMPTION)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "High Energy Usage",
                   Description = $"Electricity consumption is above normal: {waterData.ElectricityConsumption:F2}",
                    Timestamp = DateTime.Now,
                    WaterDataId = waterData.Id,
                    WaterData = waterData
                });
            }

            // Check for leaks
            if (waterData.LeakDetected == true)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Leak Detected",
                    Description = $"Leak detected at location: {waterData.Location}",
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
            if (metrics.WaterEfficiencyRatio < MIN_WATER_EFFICIENCY)
            {
                abnormalities.Add(new Abnormality
                {
                    Type = "Low Water Efficiency",
                    Description = $"Water efficiency ratio is critically low: {metrics.WaterEfficiencyRatio:F2}",
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

            return abnormalities;
        }
    }



}