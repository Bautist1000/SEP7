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
    }
}