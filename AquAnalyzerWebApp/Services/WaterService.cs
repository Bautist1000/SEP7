using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using AquAnalyzerWebApp.Models;
using Microsoft.Extensions.Logging;

public class WaterService : IWaterService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WaterService> _logger;

    public WaterService(HttpClient httpClient, ILogger<WaterService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // Water Data Methods
    public async Task<WaterDataDto> GetWaterDataByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/waterdata/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<WaterDataDto>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to retrieve water data. No data with that id exists.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to retrieve water data. {ex.Message}");
        }
    }

    public async Task<IEnumerable<WaterDataDto>> GetAllWaterDataAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/waterdata");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<WaterDataDto>>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to retrieve water data.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to retrieve water data. {ex.Message}");
        }
    }

    public async Task<WaterDataDto> AddWaterDataAsync(WaterDataDto data)
    {
        try
        {
            Console.WriteLine($"Sending POST request to: {_httpClient.BaseAddress}api/waterdata");
            Console.WriteLine($"Request content: {System.Text.Json.JsonSerializer.Serialize(data)}");

            var response = await _httpClient.PostAsJsonAsync("api/waterdata", data);
            Console.WriteLine($"Response status code: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {responseContent}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterDataDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add water data: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }


    public async Task<WaterDataDto> UpdateWaterDataAsync(int id, WaterDataDto data)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/waterdata/{id}", data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<WaterDataDto>();
        if (result == null)
        {
            throw new NullReferenceException("Failed to update water data. No data with that id exists.");
        }
        return result;
    }

    public async Task<bool> DeleteWaterDataAsync(int id)
    {
        try
        {
            Console.WriteLine($"Sending DELETE request for id: {id}");
            var response = await _httpClient.DeleteAsync($"api/waterdata/{id}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Delete response: {response.StatusCode}, Content: {content}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete exception: {ex}");
            throw;
        }
    }

    // Water Metrics Methods
    public async Task<IEnumerable<WaterMetricsDto>> GetAllMetricsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/watermetrics");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<WaterMetricsDto>>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to retrieve water metrics.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to retrieve water metrics. {ex.Message}");
        }
    }

    public async Task<WaterMetricsDto> GetMetricsByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/watermetrics/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to retrieve metrics by id. No metrics with that id exists.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to retrieve metrics by id. {ex.Message}");
        }
    }

    public async Task<WaterMetricsDto> AddMetricsAsync(WaterMetricsDto metrics)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to add metrics. {ex.Message}");
        }
    }

    public async Task<WaterMetricsDto> UpdateMetricsAsync(WaterMetricsDto metrics)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/watermetrics/{metrics.Id}", metrics);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
        if (result == null)
        {
            throw new NullReferenceException("Failed to update metrics. No metrics with that id exists.");
        }
        return result;
    }

    public async Task<bool> DeleteMetricsAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/watermetrics/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to delete metrics. {ex.Message}");
        }
    }

    public async Task GenerateMetricsAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/generate", waterData);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to generate metrics: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to generate metrics. {ex.Message}");
        }
    }

    public async Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/average-flowrate", waterData);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<double>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to calculate average flow rate.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to calculate average flow rate. {ex.Message}");
        }
    }

    public async Task<int> CountAbnormalitiesAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/count-abnormalities", waterData);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<int>();
            if (result == null)
            {
                throw new NullReferenceException("Failed to count abnormalities.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Failed to count abnormalities. {ex.Message}");
        }
    }

    public WaterMetricsDto CalculateMetrics(List<WaterDataDto> waterData)
    {
        // Convert DTOs to models
        var waterDataModels = waterData.Select(dto => new WaterData
        {
            Id = dto.Id,
            Timestamp = dto.Timestamp,
            UsageVolume = dto.UsageVolume,
            FlowRate = dto.FlowRate,
            ElectricityConsumption = dto.ElectricityConsumption,
            ProductId = dto.ProductId,
            SourceType = dto.SourceType,
            LeakDetected = dto.LeakDetected,
            Location = dto.Location,
            HasAbnormalities = dto.HasAbnormalities,
            UsesCleanEnergy = dto.UsesCleanEnergy
        }).ToList();

        // Create a new WaterMetrics model and calculate metrics
        var metrics = new WaterMetrics
        {
            DateGeneratedOn = DateTime.UtcNow
        };
        metrics.CalculateMetrics(waterDataModels);

        // Convert the model back to a DTO
        return new WaterMetricsDto
        {
            DateGeneratedOn = metrics.DateGeneratedOn,
            LeakageRate = metrics.LeakageRate,
            WaterEfficiencyRatio = metrics.WaterEfficiencyRatio,
            TotalWaterConsumption = metrics.TotalWaterConsumption,
            TotalWaterSaved = metrics.TotalWaterSaved,
            RecycledWaterUsage = metrics.RecycledWaterUsage
        };
    }
}