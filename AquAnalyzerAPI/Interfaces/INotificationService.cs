using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> AddNotification(Notification notification);
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<Notification> GetNotificationById(int id);
        Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId);
        Task<bool> UpdateNotificationStatus(int id, string status);
        Task<bool> MarkNotificationAsRead(int id, DateTime timestamp);
    }
}