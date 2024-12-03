public class Notification
{
    
    public int Id { get; set; }
    public required string Message { get; set; }
    public int UserId { get; set; } // Foreign key reference to User model
    public required string Type { get; set; } // Type of notification (e.g., Info, Warning, Error)
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } //Do we want a log of notifs?
    public DateTime? ReadAt { get; set; } //idk if this one is necessary ^
    // Optional: Additional metadata or context
    public string Metadata { get; set; }  
}
