using AquAnalyzerAPI.Models;

namespace AquAnalyzerWebApp.Interfaces
{
    public interface INotificationsService
    {
        // Notifications
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<Notification> GetNotificationById(int id);
        Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId);
        Task AddNotification(Notification notification);
        Task CreateNotificationFromAbnormality(Abnormality abnormality);
        Task<bool> MarkNotificationAsRead(int notificationId, DateTime readAt);
        Task<bool> UpdateNotificationStatus(int notificationId, bool isResolved);
        Task<IEnumerable<Notification>> GetNotificationsByType(string type);
        Task<bool> DeleteNotification(int id);

        // Abnormalities
        Task<Abnormality> AddAbnormality(Abnormality abnormality);
        Task<IEnumerable<Abnormality>> GetAllAbnormalities();
        Task<Abnormality> GetAbnormalityById(int id);
        Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type);
        Task<bool> MarkAbnormalityAsDealtWith(int id);
        Task<bool> UpdateAbnormality(int id, string description, string type);
        Task<bool> DeleteAbnormality(int id);
        Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId);
        Task<IEnumerable<Abnormality>> CheckWaterMetricsAbnormalities(int metricsId);
    }
}