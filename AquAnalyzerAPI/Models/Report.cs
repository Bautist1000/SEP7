using System;
using System.Collections.Generic;

namespace AquAnalyzerAPI.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public List<VisualisationData> Visualisations { get; set; } = new List<VisualisationData>();
        public VisualDesigner? VisualDesigner { get; set; } = null!;

        public Report()
        {
        }

        public Report(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public Report(int id, string title, string description, int userId, DateTime generatedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            UserId = userId;
            GeneratedDate = generatedDate;
        }
    }
}