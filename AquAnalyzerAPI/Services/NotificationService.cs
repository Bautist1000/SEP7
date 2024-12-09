using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _context;

        public NotificationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddNotification(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateNotificationStatus(int id, string status)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            notification.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkNotificationAsRead(int id, DateTime readAt)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            notification.ReadAt = readAt;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsByType(string type)
        {
            return await _context.Notifications
                .Where(n => n.Type == type)
                .ToListAsync();
        }
    }
}
