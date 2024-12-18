using System.Collections.Generic;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;

public interface IWaterService
{
    // Water Data 
    Task<WaterData> GetWaterDataByIdAsync(int id);
    Task<IEnumerable<WaterData>> GetAllWaterDataAsync();
    Task AddWaterDataAsync(WaterData data);
    Task UpdateWaterDataAsync(WaterData data);
    Task DeleteWaterDataAsync(int id);

    // Water Metrics 
    Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync();
    Task<WaterMetrics> GetMetricsByIdAsync(int id);
    Task GenerateMetricsAsync(IEnumerable<WaterData> waterData);
    Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData);
    Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData);
    Task AddMetricsAsync(WaterMetrics metrics);
    Task UpdateMetricsAsync(WaterMetrics metrics);
    Task DeleteMetricsAsync(int id);
}