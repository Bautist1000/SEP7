using System.Collections.Generic;
using System.Threading.Tasks;
using AquAnalyzerWebApp.Models;

public interface IWaterService
{
    // Water Data Methods
    Task<WaterDataDto> GetWaterDataByIdAsync(int id);
    Task<IEnumerable<WaterDataDto>> GetAllWaterDataAsync();
    Task<WaterDataDto> AddWaterDataAsync(WaterDataDto data);
    Task<WaterDataDto> UpdateWaterDataAsync(int id, WaterDataDto data);
    Task<bool> DeleteWaterDataAsync(int id);

    // Water Metrics Methods
    Task<IEnumerable<WaterMetricsDto>> GetAllMetricsAsync();
    Task<WaterMetricsDto> GetMetricsByIdAsync(int id);
    Task<WaterMetricsDto> AddMetricsAsync(WaterMetricsDto metrics);
    Task<WaterMetricsDto> UpdateMetricsAsync(WaterMetricsDto metrics);
    Task<bool> DeleteMetricsAsync(int id);
    Task GenerateMetricsAsync(IEnumerable<WaterDataDto> waterData);
    Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterDataDto> waterData);
    Task<int> CountAbnormalitiesAsync(IEnumerable<WaterDataDto> waterData);

    // Calculate Metrics
    WaterMetricsDto CalculateMetrics(List<WaterDataDto> waterData);
}