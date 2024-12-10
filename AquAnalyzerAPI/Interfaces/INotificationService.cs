using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotifications();

        Task<Notification> GetNotificationById(int id);

        Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId);

        Task AddNotification(Notification notification);

        Task<bool> UpdateNotificationStatus(int id, string status);

        Task<bool> MarkNotificationAsRead(int id, DateTime readAt);

        Task<IEnumerable<Notification>> GetNotificationsByType(string type);

        Task<bool> DeleteNotification(int id);
    }
}