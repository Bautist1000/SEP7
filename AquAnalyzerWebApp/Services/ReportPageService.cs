using AquAnalyzerAPI.Models;
using AquAnalyzerWebApp.Interfaces;

namespace AquAnalyzerWebApp.Services
{
    public class ReportPageService : IReportPageService
    {
        private readonly HttpClient _httpClient;

        public ReportPageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to get a report by ID
        public async Task<Report> GetReportById(int id)
        {
            var response = await _httpClient.GetAsync($"api/report/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Report>();
        }

        // Method to get all reports
        public async Task<IEnumerable<Report>> GetAllReports()
        {
            var response = await _httpClient.GetAsync("api/report");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Report>>();
        }

        // Method to add a new report
        public async Task<Report> AddReport(Report report)
        {
            var response = await _httpClient.PostAsJsonAsync("api/report", report);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Report>();
        }

        // Method to update an existing report
        public async Task UpdateReport(Report report)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/report/{report.Id}", report);
            response.EnsureSuccessStatusCode();
        }

        // Method to delete a report by ID
        public async Task DeleteReport(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/report/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}