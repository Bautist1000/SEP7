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

        // New chart-specific methods
        Task<IEnumerable<WaterData>> GetWaterDataForChart(int visualisationId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<WaterMetrics>> GetMetricsForChart(int visualisationId, DateTime? startDate = null, DateTime? endDate = null);
        Task UpdateChartType(int visualisationId, string chartType);
    }
}