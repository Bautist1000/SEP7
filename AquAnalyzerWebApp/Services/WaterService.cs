using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;

public class WaterService : IWaterService
{
    private readonly HttpClient _httpClient;

    public WaterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Water Data Methods
    public async Task<WaterData> GetWaterDataByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<WaterData>($"api/waterdata/{id}");
    }

    public async Task<IEnumerable<WaterData>> GetAllWaterDataAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<WaterData>>("api/waterdata");
    }

    public async Task AddWaterDataAsync(WaterData data)
    {
        await _httpClient.PostAsJsonAsync("api/waterdata", data);
    }

    public async Task UpdateWaterDataAsync(WaterData data)
    {
        await _httpClient.PutAsJsonAsync($"api/waterdata/{data.Id}", data);
    }

    public async Task DeleteWaterDataAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/waterdata/{id}");
    }

    // Water Metrics Methods
    public async Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<WaterMetrics>>("api/watermetrics");
    }

    public async Task<WaterMetrics> GetMetricsByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<WaterMetrics>($"api/watermetrics/{id}");
    }

    public async Task GenerateMetricsAsync(IEnumerable<WaterData> waterData)
    {
        await _httpClient.PostAsJsonAsync("api/watermetrics/generate", waterData);
    }

    public async Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics/average-flow-rate", waterData);
        return await response.Content.ReadFromJsonAsync<double>();
    }

    public async Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics/count-abnormalities", waterData);
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task AddMetricsAsync(WaterMetrics metrics)
    {
        await _httpClient.PostAsJsonAsync("api/watermetrics", metrics);
    }

    public async Task UpdateMetricsAsync(WaterMetrics metrics)
    {
        await _httpClient.PutAsJsonAsync($"api/watermetrics/{metrics.Id}", metrics);
    }

    public async Task DeleteMetricsAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/watermetrics/{id}");
    }
}