namespace AquAnalyzerAPI.Models
{
    public class Notification
    {

        public int Id { get; set; }
        public required string Message { get; set; }
        public int UserId { get; set; } // Foreign key reference to User model
        public required string Type { get; set; } // Type of notification (e.g., Info, Warning, Error)
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } //Do we want a log of notifs?
        public DateTime? ReadAt { get; set; } //idk if this one is necessary ^
        public string Metadata { get; set; }
        public int? AbnormalityId { get; set; } 
        public Abnormality? Abnormality { get; set; }
        public Notification(int Id, string Message, int UserId, string Type, string Status, DateTime CreatedAt, DateTime? ReadAt, string Metadata)
        {
            this.Id = Id;
            this.Message = Message;
            this.UserId = UserId;
            this.Type = Type;
            this.Status = Status;
            this.CreatedAt = CreatedAt;
            this.ReadAt = ReadAt;
            this.Metadata = Metadata;
        }
        
    }
}
