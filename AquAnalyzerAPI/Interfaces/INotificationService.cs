using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace AquAnalyzerAPI.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<Notification> GetNotificationById(int id);
        Task AddNotification(Notification notification);
        Task<bool> UpdateNotificationStatus(int id, string status);
        Task<bool> DeleteNotification(int id);
        Task<bool> MarkAsRead(int id, DateTime readAt);
        Task<bool> UpdateStatus(int id, bool isResolved);
        Task<IEnumerable<Notification>> GetNotificationsByType(string type);
        Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId);
        Task CreateNotificationFromAbnormality(Abnormality abnormality);


    }
}
