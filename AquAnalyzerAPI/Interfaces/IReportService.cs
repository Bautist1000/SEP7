using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Interfaces
{
    public interface IReportService
    {
        Task<Report> AddReport(Report report);
        Task<Report> GetReportById(int id);
        Task<IEnumerable<Report>> GetAllReports();
        Task UpdateReport(Report updatedReport);
        Task<bool> DeleteReport(int id);
        Task<IEnumerable<Report>> SearchReportsByTitle(string searchTerm);
    }

}
