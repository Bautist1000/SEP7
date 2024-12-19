using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class VisualisationService : IVisualisationService
    {
        private readonly DatabaseContext _context;

        public VisualisationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<VisualisationData> AddVisualisation(VisualisationData visualisation)
        {
            await _context.Visualisations.AddAsync(visualisation);
            await _context.SaveChangesAsync();
            return visualisation;
        }

        public async Task<VisualisationData> GetVisualisationById(int id)
        {
            return await _context.Visualisations
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VisualisationData>> GetAllVisualisations()
        {
            return await _context.Visualisations.ToListAsync();
        }

        public async Task<IEnumerable<VisualisationData>> GetVisualisationsByReportId(int reportId)
        {
            return await _context.Visualisations
                .Where(v => v.ReportId == reportId)
                .ToListAsync();
        }

        public async Task UpdateVisualisation(VisualisationData updatedVisualisation)
        {
            var visualisationToUpdate = await _context.Visualisations.FirstOrDefaultAsync(v => v.Id == updatedVisualisation.Id);
            if (visualisationToUpdate != null)
            {
                visualisationToUpdate.Type = updatedVisualisation.Type;
                visualisationToUpdate.MetricsUsed = updatedVisualisation.MetricsUsed;
                visualisationToUpdate.RawDataUsed = updatedVisualisation.RawDataUsed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteVisualisation(int id)
        {
            var visualisation = await _context.Visualisations.FirstOrDefaultAsync(v => v.Id == id);
            if (visualisation != null)
            {
                _context.Visualisations.Remove(visualisation);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<VisualisationData>> SearchVisualisationsByType(string searchTerm)
        {
            return await _context.Visualisations
                .Where(v => v.Type.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<WaterData>> GetWaterDataForChart(int visualisationId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Visualisations
                .Where(v => v.Id == visualisationId)
                .SelectMany(v => v.RawDataUsed);

            if (startDate.HasValue)
                query = query.Where(d => d.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(d => d.Timestamp <= endDate.Value);

            return await query
                .OrderBy(d => d.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<WaterMetrics>> GetMetricsForChart(int visualisationId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Visualisations
                .Where(v => v.Id == visualisationId)
                .SelectMany(v => v.MetricsUsed);

            if (startDate.HasValue)
                query = query.Where(m => m.DateGeneratedOn >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.DateGeneratedOn <= endDate.Value);

            return await query
                .OrderBy(m => m.DateGeneratedOn)
                .ToListAsync();
        }

        public async Task UpdateChartType(int visualisationId, string newChartType)
        {
            var visualisation = await _context.Visualisations
                .FirstOrDefaultAsync(v => v.Id == visualisationId);

            if (visualisation != null)
            {
                visualisation.Type = newChartType;
                await _context.SaveChangesAsync();
            }
        }
    }
}