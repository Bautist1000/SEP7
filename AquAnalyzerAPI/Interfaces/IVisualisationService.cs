using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IVisualisationService
    {
        Task<Visualisation> AddVisualisation(Visualisation visualisation);
        Task<Visualisation> GetVisualisationById(int id);
        Task<IEnumerable<Visualisation>> GetAllVisualisations();
        Task<IEnumerable<Visualisation>> GetVisualisationsByReportId(int reportId);
        Task UpdateVisualisation(Visualisation updatedVisualisation);
        Task<bool> DeleteVisualisation(int id);
        Task<IEnumerable<Visualisation>> SearchVisualisationsByType(string searchTerm);
    }
}
