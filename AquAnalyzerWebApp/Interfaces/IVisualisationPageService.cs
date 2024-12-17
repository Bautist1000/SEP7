using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquAnalyzerWebApp.Interfaces
{
    public interface IVisualisationPageService
    {
        Task<Visualisation> GetVisualisationById(int id);
        Task<IEnumerable<Visualisation>> GetAllVisualisations();
        Task<IEnumerable<Visualisation>> GetVisualisationsByReportId(int reportId);
        Task<Visualisation> AddVisualisation(Visualisation visualisation);
        Task UpdateVisualisation(Visualisation visualisation);
        Task DeleteVisualisation(int id);
    }
}