using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
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

        public async Task CreateNotificationFromAbnormality(Abnormality abnormality)
        {
            var notification = new Notification
            {
                Message = $"New abnormality detected: {abnormality.Description}",
                Type = "Abnormality",
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                ReadAt = null,
                UserId = 1, // Set appropriate user ID for analyst
                Metadata = "{}",
                AbnormalityId = abnormality.Id,
                Abnormality = abnormality
            };

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
                .Include(n => n.Abnormality)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatus(int id, bool isResolved)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var notification = await _context.Notifications
                    .Include(n => n.Abnormality)
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (notification == null)
                    return false;

                // Update notification status
                notification.Status = isResolved ? "Resolved" : "Active";

                // If there's an associated abnormality, update its status too
                if (notification.Abnormality != null)
                {
                    notification.Abnormality.IsDealtWith = isResolved;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> MarkAsRead(int id, DateTime readAt)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return false;

            notification.ReadAt = readAt;
            await _context.SaveChangesAsync();
            return true;
        }

        // Add helper method to check if abnormality is resolved
        private async Task CheckAndUpdateAbnormalityStatus(int abnormalityId)
        {
            var abnormality = await _context.Abnormalities
                .Include(a => a.WaterData)
                .FirstOrDefaultAsync(a => a.Id == abnormalityId);

            if (abnormality != null && abnormality.WaterData != null)
            {
                // Check if conditions that caused abnormality are resolved
                var isResolved = !await _context.Abnormalities
                    .AnyAsync(a => a.WaterDataId == abnormality.WaterDataId && !a.IsDealtWith);

                if (isResolved)
                {
                    abnormality.IsDealtWith = true;
                    abnormality.WaterData.HasAbnormalities = false;
                    await _context.SaveChangesAsync();

                    // Update related notification
                    var notification = await _context.Notifications
                        .FirstOrDefaultAsync(n => n.AbnormalityId == abnormalityId);
                    if (notification != null)
                    {
                        notification.Status = "Resolved";
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<bool> UpdateNotificationStatus(int id, string status)
        {
            var notification = await _context.Notifications
                .Include(n => n.Abnormality)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notification == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                notification.Status = status;

                if (notification.Abnormality != null && status == "Resolved")
                {
                    notification.Abnormality.IsDealtWith = true;
                    if (notification.Abnormality.WaterData != null)
                    {
                        await CheckAndUpdateAbnormalityStatus(notification.Abnormality.Id);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> MarkNotificationAsRead(int id, DateTime readAt)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return false;

            notification.ReadAt = readAt;
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


        public async Task<IEnumerable<Notification>> GetNotificationsByType(string type)
        {
            return await _context.Notifications
                .Where(n => n.Type == type)
                .ToListAsync();
        }
    }
}
