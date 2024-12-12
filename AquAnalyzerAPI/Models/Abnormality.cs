namespace AquAnalyzerAPI.Models
{
    public class Abnormality
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsDealtWith { get; set; } = false; 
        public int? WaterDataId { get; set; }
        public WaterData? WaterData { get; set; }

        public int? WaterMetricsId { get; set; }
        public WaterMetrics? WaterMetrics { get; set; }

        public Abnormality()
        {
        }
        public Abnormality(int Id, DateTime Timestamp, string Description, string Type)
        {
            this.Id = Id;
            this.Timestamp = Timestamp;
            this.Description = Description;
            this.Type = Type;
        }
    }
}

