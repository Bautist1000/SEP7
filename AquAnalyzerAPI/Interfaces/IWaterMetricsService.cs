using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IWaterMetricsService
    {
        Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync();
        Task<WaterMetrics> GetMetricsByIdAsync(int id);
        Task GenerateMetricsAsync(IEnumerable<WaterData> waterData);
        Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData);
        Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData);
        Task AddMetricsAsync(WaterMetrics metrics);
        Task UpdateMetricsAsync(WaterMetrics metrics);
        Task DeleteMetricsAsync(int id);
    }
}


