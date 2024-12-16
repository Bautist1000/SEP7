namespace AquAnalyzerAPI.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public List<Visualisation> Visualisations { get; set; } = new List<Visualisation>();
        public VisualDesigner VisualDesigner { get; set; } = null!;

        public Report()
        {

        }

        public Report(int Id, string Title, string Description)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
        }

        public Report(int Id, string Title, string Description, int UserId, DateTime GeneratedDate)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.UserId = UserId;
            this.GeneratedDate = GeneratedDate;
        }
    }
}
