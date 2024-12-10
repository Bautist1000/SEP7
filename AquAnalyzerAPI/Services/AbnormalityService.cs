using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class AbnormalityService : IAbnormalityService
    {
        private readonly DatabaseContext _context;

        public AbnormalityService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Abnormality> AddAbnormality(Abnormality abnormality)
        {
            await _context.Abnormalities.AddAsync(abnormality);
            await _context.SaveChangesAsync();
            return abnormality;
        }

        public async Task<IEnumerable<Abnormality>> GetAllAbnormalities()
        {
            return await _context.Abnormalities.ToListAsync();
        }

        public async Task<Abnormality> GetAbnormalityById(int id)
        {
            return await _context.Abnormalities.FindAsync(id);
        }

        public async Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type)
        {
            return await _context.Abnormalities
                .Where(a => a.Type == type)
                .ToListAsync();
        }

        public async Task<bool> UpdateAbnormality(int id, string description, string type)
        {
            var abnormality = await _context.Abnormalities.FindAsync(id);
            if (abnormality == null)
            {
                return false;
            }

            abnormality.Description = description;
            abnormality.Type = type;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAbnormality(int id)
        {
            var abnormality = await _context.Abnormalities.FindAsync(id);
            if (abnormality == null)
            {
                return false;
            }

            _context.Abnormalities.Remove(abnormality);
            await _context.SaveChangesAsync();
            return true;
        }
    }


}