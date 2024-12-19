using AquAnalyzerAPI.Dtos;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IVisualisationService
    {
        Task<VisualisationDataDto> AddVisualisation(VisualisationData visualisation);
        Task<VisualisationDataDto> GetVisualisationById(int id);
        Task<IEnumerable<VisualisationDataDto>> GetAllVisualisations();
        Task<IEnumerable<VisualisationDataDto>> GetVisualisationsByReportId(int reportId);
        Task<VisualisationDataDto> UpdateVisualisation(VisualisationData updatedVisualisation);
        Task<bool> DeleteVisualisation(int id);
        Task<IEnumerable<WaterDataDto>> GetWaterDataForChart(int visualisationId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<WaterMetricsDto>> GetMetricsForChart(int id, DateTime? startDate, DateTime? endDate);
        Task<VisualisationDataDto> UpdateChartType(int id, string chartType);

    }
}
