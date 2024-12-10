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
