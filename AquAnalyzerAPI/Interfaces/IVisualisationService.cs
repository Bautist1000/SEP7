using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IVisualisationService
    {
        Task<VisualisationData> AddVisualisation(VisualisationData visualisation);
        Task<VisualisationData> GetVisualisationById(int id);
        Task<IEnumerable<VisualisationData>> GetAllVisualisations();
        Task<IEnumerable<VisualisationData>> GetVisualisationsByReportId(int reportId);
        Task UpdateVisualisation(VisualisationData updatedVisualisation);
        Task<bool> DeleteVisualisation(int id);
        Task<IEnumerable<VisualisationData>> SearchVisualisationsByType(string searchTerm);
    }
}
