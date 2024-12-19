// ChartConfiguration.cs
namespace AquAnalyzerAPI.Models
{
    public class ChartConfiguration
    {
        public int Id { get; set; }
        public int VisualisationDataId { get; set; }
        public string Title { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string ColorScheme { get; set; }
        public bool ShowLegend { get; set; } = true;
        public bool ShowGrid { get; set; } = true;

        public ChartConfiguration()
        {
            ColorScheme = "default";
            Title = "Water Usage Chart";
            XAxisLabel = "Time";
            YAxisLabel = "Usage (m³)";
        }
    }
}

