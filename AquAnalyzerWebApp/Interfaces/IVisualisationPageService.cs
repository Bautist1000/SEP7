using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquAnalyzerWebApp.Interfaces
{
    public interface IVisualisationPageService
    {
        Task<VisualisationData> GetVisualisationById(int id);
        Task<IEnumerable<VisualisationData>> GetAllVisualisations();
        Task<IEnumerable<VisualisationData>> GetVisualisationsByReportId(int reportId);
        Task<VisualisationData> AddVisualisation(VisualisationData visualisation);
        Task UpdateVisualisation(VisualisationData visualisation);
        Task DeleteVisualisation(int id);
    }
}