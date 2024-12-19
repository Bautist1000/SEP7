using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using AquAnalyzerWebApp.Interfaces;

namespace AquAnalyzerWebApp.Services
{
    public class VisualisationPageService : IVisualisationPageService
    {
        private readonly HttpClient _httpClient;

        public VisualisationPageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VisualisationData> GetVisualisationById(int id)
        {
            var response = await _httpClient.GetAsync($"api/visualisation/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VisualisationData>() ?? new VisualisationData(id, string.Empty, 0);
        }

        public async Task<IEnumerable<VisualisationData>> GetAllVisualisations()
        {
            var response = await _httpClient.GetAsync("api/visualisation");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<VisualisationData>>() ?? new List<VisualisationData>();
        }

        public async Task<IEnumerable<VisualisationData>> GetVisualisationsByReportId(int reportId)
        {
            var response = await _httpClient.GetAsync($"api/visualisation/report/{reportId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<VisualisationData>>() ?? new List<VisualisationData>();
        }

        public async Task AddVisualisation(VisualisationData visualisation)
        {
            var response = await _httpClient.PostAsJsonAsync("api/visualisation", visualisation);
            response.EnsureSuccessStatusCode();
            // return await response.Content.ReadFromJsonAsync<VisualisationData>() ?? new VisualisationData(0, string.Empty, 0);
        }

        public async Task UpdateVisualisation(VisualisationData visualisation)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/visualisation/{visualisation.Id}", visualisation);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteVisualisation(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/visualisation/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<WaterData>> GetWaterDataForChart(int visualisationId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var url = $"api/visualisation/{visualisationId}/waterdata";
            if (startDate.HasValue && endDate.HasValue)
            {
                url += $"?startDate={startDate.Value:yyyy-MM-dd}&endDate={endDate.Value:yyyy-MM-dd}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterData>>() ?? new List<WaterData>();
        }

        public async Task<IEnumerable<WaterMetrics>> GetMetricsForChart(int visualisationId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var url = $"api/visualisation/{visualisationId}/metrics";
            if (startDate.HasValue && endDate.HasValue)
            {
                url += $"?startDate={startDate.Value:yyyy-MM-dd}&endDate={endDate.Value:yyyy-MM-dd}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterMetrics>>() ?? new List<WaterMetrics>();
        }

        public async Task UpdateChartType(int visualisationId, string chartType)
        {
            var content = new StringContent($"\"{chartType}\"", System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/visualisation/{visualisationId}/charttype", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<WaterData>> GetAllWaterDataAsync()
        {
            var response = await _httpClient.GetAsync("api/waterdata");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterData>>() ?? new List<WaterData>();
        }

        public async Task<IEnumerable<WaterMetrics>> GetAllWaterMetricsAsync()
        {
            var response = await _httpClient.GetAsync("api/watermetrics");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<WaterMetrics>>() ?? new List<WaterMetrics>();
        }

    }
}