using AquAnalyzerAPI.Models;

namespace AquAnalyzerWebApp.Interfaces
{
    public interface IReportPageService
    {
        Task<Report> GetReportById(int id);
        Task<IEnumerable<Report>> GetAllReports();
        Task<Report> AddReport(Report report);
        Task UpdateReport(Report report);
        Task DeleteReport(int id);
    }
}