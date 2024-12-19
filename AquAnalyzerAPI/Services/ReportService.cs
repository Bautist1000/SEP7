using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Dtos;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseContext _context;

        public ReportService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ReportDto> AddReport(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return MapToDto(report);
        }

        public async Task<ReportDto> GetReportById(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Visualisations)
                    .ThenInclude(v => v.MetricsUsed)
                .Include(r => r.Visualisations)
                    .ThenInclude(v => v.RawDataUsed)
                .Include(r => r.Visualisations)
                    .ThenInclude(v => v.ChartConfig)
                .FirstOrDefaultAsync(r => r.Id == id);

            return MapToDto(report);
        }

        public async Task<IEnumerable<ReportDto>> GetAllReports()
        {
            var reports = await _context.Reports
                .Include(r => r.Visualisations)
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        public async Task<ReportDto> UpdateReport(Report updatedReport)
        {
            var reportToUpdate = await _context.Reports
                .Include(r => r.Visualisations)
                .FirstOrDefaultAsync(r => r.Id == updatedReport.Id);

            if (reportToUpdate != null)
            {
                reportToUpdate.Title = updatedReport.Title;
                reportToUpdate.Description = updatedReport.Description;

                foreach (var vis in updatedReport.Visualisations)
                {
                    var existingVis = reportToUpdate.Visualisations
                        .FirstOrDefault(v => v.Id == vis.Id);

                    if (existingVis == null)
                        reportToUpdate.Visualisations.Add(vis);
                }

                await _context.SaveChangesAsync();
                return MapToDto(reportToUpdate);
            }

            return null;
        }

        public async Task<bool> DeleteReport(int id)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ReportDto>> SearchReportsByTitle(string searchTerm)
        {
            var reports = await _context.Reports
                .Where(r => r.Title.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();

            return reports.Select(MapToDto);
        }

        private static ReportDto MapToDto(Report report)
        {
            if (report == null) return null;

            return new ReportDto
            {
                Id = report.Id,
                Title = report.Title,
                Description = report.Description,
                GeneratedDate = report.GeneratedDate,
                Visualisations = report.Visualisations?.Select(v => new VisualisationDataDto
                {
                    Id = v.Id,
                    Type = v.Type,
                    ChartConfig = v.ChartConfig != null ? new ChartConfiguration
                    {
                        Title = v.ChartConfig.Title,
                        XAxisLabel = v.ChartConfig.XAxisLabel,
                        YAxisLabel = v.ChartConfig.YAxisLabel,
                        ColorScheme = v.ChartConfig.ColorScheme,
                        ShowLegend = v.ChartConfig.ShowLegend,
                        ShowGrid = v.ChartConfig.ShowGrid
                    } : null,
                    RawDataUsed = v.RawDataUsed?.Select(w => new WaterDataDto
                    {
                        Id = w.Id,
                        Timestamp = w.Timestamp,
                        UsageVolume = w.UsageVolume,
                        Location = w.Location,
                        SourceType = w.SourceType
                    }).ToList(),
                    MetricsUsed = v.MetricsUsed?.Select(m => new WaterMetricsDto
                    {
                        Id = m.Id,
                        DateGeneratedOn = m.DateGeneratedOn,
                        WaterEfficiencyRatio = m.WaterEfficiencyRatio,
                        TotalWaterConsumption = m.TotalWaterConsumption
                    }).ToList()
                }).ToList()
            };
        }
    }
}