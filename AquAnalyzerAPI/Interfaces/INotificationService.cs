


public interface INotificationService
{
    Task<IEnumerable<Notification>> GetAllNotifications(); 
    Task<Notification> GetNotificationById(int id);
    Task<List<Notification>> GetNotificationsByUserId(int userId);
    Task<Notification> AddNotification(Notification notification);
    Task<bool> UpdateNotificationStatus(int id, string status);
    Task<bool> MarkNotificationAsRead(int id, DateTime readAt);
    Task<bool> DeleteNotification(int id);
    Task<IEnumerable<Notification>> GetNotificationsByType(string type);
}
