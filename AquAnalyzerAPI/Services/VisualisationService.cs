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

        public async Task<Visualisation> AddVisualisation(Visualisation visualisation)
        {
            await _context.Visualisations.AddAsync(visualisation);
            await _context.SaveChangesAsync();
            return visualisation;
        }

        public async Task<Visualisation> GetVisualisationById(int id)
        {
            return await _context.Visualisations
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Visualisation>> GetAllVisualisations()
        {
            return await _context.Visualisations.ToListAsync();
        }

        public async Task<IEnumerable<Visualisation>> GetVisualisationsByReportId(int reportId)
        {
            return await _context.Visualisations
                .Where(v => v.ReportId == reportId)
                .ToListAsync();
        }

        public async Task UpdateVisualisation(Visualisation updatedVisualisation)
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

        public async Task<IEnumerable<Visualisation>> SearchVisualisationsByType(string searchTerm)
        {
            return await _context.Visualisations
                .Where(v => v.Type.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();
        }
    }
}
