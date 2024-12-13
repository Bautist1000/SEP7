using System.Net.Http.Json;
using AquAnalyzerAPI.Models;
using AquAnalyzerWebApp.Interfaces;

namespace AquAnalyzerWebApp.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly HttpClient _httpClient;

        public NotificationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Abnormality methods
        public async Task<Abnormality> AddAbnormality(Abnormality abnormality)
        {
            var response = await _httpClient.PostAsJsonAsync("api/abnormality", abnormality);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Abnormality>();
        }

        public async Task<IEnumerable<Abnormality>> GetAllAbnormalities()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Abnormality>>("api/abnormality");
        }

        public async Task<Abnormality> GetAbnormalityById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Abnormality>($"api/abnormality/{id}");
        }

        public async Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Abnormality>>($"api/abnormality/type/{type}");
        }

        public async Task<bool> MarkAbnormalityAsDealtWith(int id)
        {
            var response = await _httpClient.PutAsJsonAsync<object>($"api/abnormality/{id}/dealt-with", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAbnormality(int id, string description, string type)
        {
            var abnormality = new Abnormality { Id = id, Description = description, Type = type };
            var response = await _httpClient.PutAsJsonAsync($"api/abnormality/{id}", abnormality);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAbnormality(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/abnormality/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Abnormality>>($"api/abnormality/check-water-data/{dataId}");
        }

        public async Task<IEnumerable<Abnormality>> CheckWaterMetricsAbnormalities(int metricsId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Abnormality>>($"api/abnormality/check-water-metrics/{metricsId}");
        }

        // Notification methods
        public async Task AddNotification(Notification notification)
        {
            var response = await _httpClient.PostAsJsonAsync("api/notification", notification);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Notification>> GetAllNotifications()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Notification>>("api/notification");
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Notification>($"api/notification/{id}");
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Notification>>($"api/notification/user/{userId}");
        }

        public async Task<bool> UpdateNotificationStatus(int id, string status)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notification/{id}/status", new { status });
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> MarkNotificationAsRead(int id, System.DateTime readAt)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notification/{id}/read", new { readAt });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteNotification(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/notification/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByType(string type)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Notification>>($"api/notification/type/{type}");
        }


    }
}