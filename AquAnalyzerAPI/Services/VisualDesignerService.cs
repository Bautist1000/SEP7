using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class VisualDesignerService : IVisualDesignerService
    {
        private readonly DatabaseContext _context;

        public VisualDesignerService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VisualDesigner>> GetAllVisDesig()
        {
            return await _context.VisualDesigners.ToListAsync();
        }

        public async Task<VisualDesigner> GetByIdOfVisDesig(int id)
        {
            var visualDesigner = await _context.VisualDesigners.FindAsync(id);
            if (visualDesigner == null)
            {
                throw new KeyNotFoundException($"VisualDesigner with id {id} not found.");
            }
            return visualDesigner;
        }

        public async Task<VisualDesigner> AddVisDesig(VisualDesigner visualDesigner)
        {
            _context.VisualDesigners.Add(visualDesigner);
            await _context.SaveChangesAsync();
            return visualDesigner;
        }

        public async Task<VisualDesigner> UpdateVisDesig(VisualDesigner visualDesigner)
        {
            _context.Entry(visualDesigner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return visualDesigner;
        }

        public async Task DeleteVisDesig(int id)
        {
            var visualDesigner = await _context.VisualDesigners.FindAsync(id);
            if (visualDesigner != null)
            {
                _context.VisualDesigners.Remove(visualDesigner);
                await _context.SaveChangesAsync();
            }
        }
    }
}
