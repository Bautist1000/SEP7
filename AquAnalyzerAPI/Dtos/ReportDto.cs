namespace AquAnalyzerAPI.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime GeneratedDate { get; set; }
        public ICollection<VisualisationDataDto> Visualisations { get; set; }
    }
}