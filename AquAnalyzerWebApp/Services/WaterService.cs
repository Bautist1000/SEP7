using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Net;
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
        var response = await _httpClient.GetAsync($"api/waterdata/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<WaterData>();
        }
        throw new HttpRequestException($"Error retrieving water data: {response.StatusCode}");
    }

    public async Task<IEnumerable<WaterData>> GetAllWaterDataAsync()
    {
        var response = await _httpClient.GetAsync("api/waterdata");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterData>>();
        }
        throw new HttpRequestException($"Error retrieving all water data: {response.StatusCode}");
    }

    public async Task AddWaterDataAsync(WaterData data)
    {
        var response = await _httpClient.PostAsJsonAsync("api/waterdata", data);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error adding water data: {response.StatusCode}");
        }
    }

    public async Task UpdateWaterDataAsync(WaterData data)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            var response = await _httpClient.PutAsJsonAsync($"api/waterdata/{data.Id}", data, options);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error updating water data: {response.StatusCode}, {content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update error details: {ex}");
            throw;
        }
    }

    public async Task DeleteWaterDataAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/waterdata/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error deleting water data: {response.StatusCode}");
        }
    }

    // Water Metrics Methods
    public async Task<IEnumerable<WaterMetrics>> GetAllMetricsAsync()
    {
        var response = await _httpClient.GetAsync("api/watermetrics");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterMetrics>>();
        }
        throw new HttpRequestException($"Error retrieving metrics: {response.StatusCode}");
    }

    public async Task<WaterMetrics> GetMetricsByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/watermetrics/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<WaterMetrics>();
        }
        throw new HttpRequestException($"Error retrieving metrics by id: {response.StatusCode}");
    }

    public async Task GenerateMetricsAsync(IEnumerable<WaterData> waterData)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics/generate", waterData);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error generating metrics: {response.StatusCode}");
        }
    }

    public async Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterData> waterData)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics/average-flowrate", waterData);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<double>();
        }
        throw new HttpRequestException($"Error calculating average flow rate: {response.StatusCode}");
    }

    public async Task<int> CountAbnormalitiesAsync(IEnumerable<WaterData> waterData)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics/count-abnormalities", waterData);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<int>();
        }
        throw new HttpRequestException($"Error counting abnormalities: {response.StatusCode}");
    }

    public async Task AddMetricsAsync(WaterMetrics metrics)
    {
        var response = await _httpClient.PostAsJsonAsync("api/watermetrics", metrics);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error adding metrics: {response.StatusCode}");
        }
    }

    public async Task UpdateMetricsAsync(WaterMetrics metrics)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/watermetrics/{metrics.Id}", metrics);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error updating metrics: {response.StatusCode}");
        }
    }

    public async Task DeleteMetricsAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/watermetrics/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error deleting metrics: {response.StatusCode}");
        }
    }
}