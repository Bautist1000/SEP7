using AquAnalyzerAPI.Dtos;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Files;
using Microsoft.EntityFrameworkCore;

namespace AquAnalyzerAPI.Services
{
    public class VisualisationService : IVisualisationService
    {
        private readonly DatabaseContext _context;

        public VisualisationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<VisualisationDataDto> AddVisualisation(VisualisationData visualisation)
        {
            await _context.Visualisations.AddAsync(visualisation);
            await _context.SaveChangesAsync();
            return MapToDto(visualisation);
        }

        public async Task<VisualisationDataDto> GetVisualisationById(int id)
        {
            var visualisation = await _context.Visualisations
                .Include(v => v.MetricsUsed)
                .Include(v => v.RawDataUsed)
                .Include(v => v.ChartConfig)
                .FirstOrDefaultAsync(v => v.Id == id);

            return MapToDto(visualisation);
        }

        public async Task<IEnumerable<VisualisationDataDto>> GetAllVisualisations()
        {
            var visualisations = await _context.Visualisations
                .Include(v => v.MetricsUsed)
                .Include(v => v.RawDataUsed)
                .Include(v => v.ChartConfig)
                .ToListAsync();

            return visualisations.Select(MapToDto);
        }

        public async Task<IEnumerable<VisualisationDataDto>> GetVisualisationsByReportId(int reportId)
        {
            var visualisations = await _context.Visualisations
                .Include(v => v.MetricsUsed)
                .Include(v => v.RawDataUsed)
                .Include(v => v.ChartConfig)
                .Where(v => v.ReportId == reportId)
                .ToListAsync();

            return visualisations.Select(MapToDto);
        }

        public async Task<VisualisationDataDto> UpdateVisualisation(VisualisationData updatedVisualisation)
        {
            var visualisation = await _context.Visualisations
                .Include(v => v.MetricsUsed)
                .Include(v => v.RawDataUsed)
                .Include(v => v.ChartConfig)
                .FirstOrDefaultAsync(v => v.Id == updatedVisualisation.Id);

            if (visualisation != null)
            {
                visualisation.Type = updatedVisualisation.Type;
                visualisation.ChartConfig = updatedVisualisation.ChartConfig;
                visualisation.MetricsUsed = updatedVisualisation.MetricsUsed;
                visualisation.RawDataUsed = updatedVisualisation.RawDataUsed;

                await _context.SaveChangesAsync();
                return MapToDto(visualisation);
            }

            return null;
        }

        public async Task<IEnumerable<WaterDataDto>> GetWaterDataForChart(int visualisationId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Visualisations
                .Where(v => v.Id == visualisationId)
                .SelectMany(v => v.RawDataUsed)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(d => d.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(d => d.Timestamp <= endDate.Value);

            var waterData = await query.OrderBy(d => d.Timestamp).ToListAsync();
            return waterData.Select(MapToWaterDataDto);
        }

        private static VisualisationDataDto MapToDto(VisualisationData visualisation)
        {
            if (visualisation == null) return null;

            return new VisualisationDataDto
            {
                Id = visualisation.Id,
                Type = visualisation.Type,
                ChartConfig = visualisation.ChartConfig != null ? new ChartConfiguration
                {
                    Title = visualisation.ChartConfig.Title,
                    XAxisLabel = visualisation.ChartConfig.XAxisLabel,
                    YAxisLabel = visualisation.ChartConfig.YAxisLabel,
                    ColorScheme = visualisation.ChartConfig.ColorScheme,
                    ShowLegend = visualisation.ChartConfig.ShowLegend,
                    ShowGrid = visualisation.ChartConfig.ShowGrid
                } : null,
                RawDataUsed = visualisation.RawDataUsed?.Select(MapToWaterDataDto).ToList(),
                MetricsUsed = visualisation.MetricsUsed?.Select(MapToWaterMetricsDto).ToList()
            };
        }

        private static WaterDataDto MapToWaterDataDto(WaterData waterData)
        {
            return new WaterDataDto
            {
                Id = waterData.Id,
                Timestamp = waterData.Timestamp,
                UsageVolume = waterData.UsageVolume,
                Location = waterData.Location,
                SourceType = waterData.SourceType
            };
        }

        public async Task<bool> DeleteVisualisation(int id)
        {
            var visualisation = await _context.Visualisations.FindAsync(id);
            if (visualisation == null)
            {
                return false;
            }

            _context.Visualisations.Remove(visualisation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WaterMetricsDto>> GetMetricsForChart(int visualisationId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Visualisations
                .Where(v => v.Id == visualisationId)
                .SelectMany(v => v.MetricsUsed)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(m => m.DateGeneratedOn >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.DateGeneratedOn <= endDate.Value);

            var metrics = await query.OrderBy(m => m.DateGeneratedOn).ToListAsync();
            return metrics.Select(MapToWaterMetricsDto);
        }

        private static WaterMetricsDto MapToWaterMetricsDto(WaterMetrics metrics)
        {
            return new WaterMetricsDto
            {
                Id = metrics.Id,
                DateGeneratedOn = metrics.DateGeneratedOn,
                WaterEfficiencyRatio = metrics.WaterEfficiencyRatio,
                TotalWaterConsumption = metrics.TotalWaterConsumption
            };
        }

        public async Task<VisualisationDataDto> UpdateChartType(int visualisationId, string newChartType)
        {
            var visualisation = await _context.Visualisations
                .Include(v => v.MetricsUsed)
                .Include(v => v.RawDataUsed)
                .Include(v => v.ChartConfig)
                .FirstOrDefaultAsync(v => v.Id == visualisationId);

            if (visualisation != null)
            {
                visualisation.Type = newChartType;
                await _context.SaveChangesAsync();
                return MapToDto(visualisation);
            }

            return null;
        }
    }
}