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
            var response = await _httpClient.GetAsync($"api/visualisations/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VisualisationData>() ?? new VisualisationData(id, string.Empty, 0);
        }

        public async Task<IEnumerable<VisualisationData>> GetAllVisualisations()
        {
            var response = await _httpClient.GetAsync("api/visualisations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<VisualisationData>>() ?? new List<VisualisationData>();
        }

        public async Task<IEnumerable<VisualisationData>> GetVisualisationsByReportId(int reportId)
        {
            var response = await _httpClient.GetAsync($"api/visualisations/report/{reportId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<VisualisationData>>() ?? new List<VisualisationData>();
        }

        public async Task<VisualisationData> AddVisualisation(VisualisationData visualisation)
        {
            var response = await _httpClient.PostAsJsonAsync("api/visualisations", visualisation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VisualisationData>() ?? new VisualisationData(0, string.Empty, 0);
        }

        public async Task UpdateVisualisation(VisualisationData visualisation)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/visualisations/{visualisation.Id}", visualisation);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteVisualisation(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/visualisations/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<WaterData>> GetWaterDataForChart(int visualisationId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var url = $"api/visualisations/{visualisationId}/waterdata";
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
            var url = $"api/visualisations/{visualisationId}/metrics";
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
            var response = await _httpClient.PutAsync($"api/visualisations/{visualisationId}/charttype", content);
            response.EnsureSuccessStatusCode();
        }

    }
}