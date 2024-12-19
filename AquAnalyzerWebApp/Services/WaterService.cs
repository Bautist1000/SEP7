using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AquAnalyzerAPI.Dtos;
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
            _logger.LogInformation($"Fetching water data by ID: {id}");
            var response = await _httpClient.GetAsync($"api/waterdata/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<WaterDataDto>();

            if (result == null)
                throw new NullReferenceException("Failed to retrieve water data. No data with that ID exists.");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching water data by ID: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<WaterDataDto>> GetAllWaterDataAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all water data.");
            var response = await _httpClient.GetAsync("api/waterdata");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<WaterDataDto>>();

            if (result == null)
                throw new NullReferenceException("Failed to retrieve water data.");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all water data.");
            throw;
        }
    }

    public async Task<WaterDataDto> AddWaterDataAsync(WaterDataDto data)
    {
        try
        {
            _logger.LogInformation($"Adding new water data: {System.Text.Json.JsonSerializer.Serialize(data)}");
            var response = await _httpClient.PostAsJsonAsync("api/waterdata", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterDataDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding water data.");
            throw;
        }
    }

    public async Task<WaterDataDto> UpdateWaterDataAsync(int id, WaterDataDto data)
    {
        try
        {
            _logger.LogInformation($"Updating water data for ID: {id}");
            var response = await _httpClient.PutAsJsonAsync($"api/waterdata/{id}", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterDataDto>() ?? data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating water data for ID: {id}");
            throw;
        }
    }

    public async Task<bool> DeleteWaterDataAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting water data for ID: {id}");
            var response = await _httpClient.DeleteAsync($"api/waterdata/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting water data for ID: {id}");
            throw;
        }
    }

    // Water Metrics Methods
    public async Task<IEnumerable<WaterMetricsDto>> GetAllMetricsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all water metrics.");
            var response = await _httpClient.GetAsync("api/watermetrics");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterMetricsDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all water metrics.");
            throw;
        }
    }

    public async Task<WaterMetricsDto> GetMetricsByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Fetching metrics by ID: {id}");
            var response = await _httpClient.GetAsync($"api/watermetrics/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching metrics by ID: {id}");
            throw;
        }
    }

    public async Task<WaterMetricsDto> AddMetricsAsync(WaterMetricsDto metrics)
    {
        try
        {
            _logger.LogInformation("Adding new water metrics.");
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding water metrics.");
            throw;
        }
    }

    public async Task<WaterMetricsDto> UpdateMetricsAsync(WaterMetricsDto metrics)
    {
        try
        {
            _logger.LogInformation($"Updating water metrics for ID: {metrics.Id}");
            var response = await _httpClient.PutAsJsonAsync($"api/watermetrics/{metrics.Id}", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WaterMetricsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating metrics for ID: {metrics.Id}");
            throw;
        }
    }

    public async Task<bool> DeleteMetricsAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting water metrics for ID: {id}");
            var response = await _httpClient.DeleteAsync($"api/watermetrics/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting metrics for ID: {id}");
            throw;
        }
    }

    public async Task GenerateMetricsAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            _logger.LogInformation("Generating water metrics.");
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/generate", waterData);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating water metrics.");
            throw;
        }
    }

    public async Task<double> CalculateAverageFlowRateAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            _logger.LogInformation("Calculating average flow rate.");
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/average-flowrate", waterData);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<double>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating average flow rate.");
            throw;
        }
    }

    public async Task<int> CountAbnormalitiesAsync(IEnumerable<WaterDataDto> waterData)
    {
        try
        {
            _logger.LogInformation("Counting abnormalities in water data.");
            var response = await _httpClient.PostAsJsonAsync("api/watermetrics/count-abnormalities", waterData);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting abnormalities.");
            throw;
        }
    }

    public WaterMetricsDto CalculateMetrics(List<WaterDataDto> waterData)
    {
        _logger.LogInformation("Calculating metrics locally.");
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

        var metrics = new WaterMetrics
        {
            DateGeneratedOn = DateTime.UtcNow
        };
        metrics.CalculateMetrics(waterDataModels);

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
