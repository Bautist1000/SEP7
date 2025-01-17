
using System.Threading.Tasks;
using AquAnalyzerAPI.Dtos;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces

{
    public interface IReportService
    {

        Task<ReportDto> AddReport(Report report);

        Task<ReportDto> GetReportById(int id);

        Task<IEnumerable<ReportDto>> GetAllReports();

        Task<ReportDto> UpdateReport(Report updatedReport);

        Task<bool> DeleteReport(int id);

        Task<IEnumerable<ReportDto>> SearchReportsByTitle(string searchTerm);

    }

}
