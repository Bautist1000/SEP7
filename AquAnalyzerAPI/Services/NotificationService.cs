

public class NotificationService : INotificationService
{
    private readonly DatabaseContext _context;

    public NotificationService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Notification> AddNotification(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification;
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
}

