using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IWaterMetricsService
    {
        Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync();
        Task<WaterMetrics> GetMetricsByIdAsync(int id);
        Task<WaterMetrics> GenerateMetricsAsync(IEnumerable<WaterData> waterData);
        Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData);
        Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData);
        Task AddMetricsAsync(WaterMetrics metrics);
        Task UpdateMetricsAsync(WaterMetrics metrics);
        Task DeleteMetricsAsync(int id);
    }
}

