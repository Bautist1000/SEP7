using Microsoft.EntityFrameworkCore;


namespace AquAnalyzerAPI.Services
{
    public class AnalystService : IAnalystService
    {
        private readonly DatabaseContext _context;

        public AnalystService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Analyst>> GetAllAnalysts()
        {
            return await _context.Analysts.ToListAsync();
        }

        public async Task<Analyst> GetAnalystById(int id)
        {
            var analyst = await _context.Analysts.FindAsync(id);
            if (analyst == null)
            {
                throw new KeyNotFoundException($"Analyst with id {id} not found.");
            }
            return analyst;
        }

        public async Task<Analyst> AddAnalyst(Analyst analyst)
        {
            _context.Analysts.Add(analyst);
            await _context.SaveChangesAsync();
            return analyst;
        }

        public async Task<Analyst> UpdateAnalyst(Analyst analyst)
        {
            _context.Entry(analyst).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return analyst;
        }

        public async Task DeleteAnalyst(int id)
        {
            var analyst = await _context.Analysts.FindAsync(id);
            if (analyst != null)
            {
                _context.Analysts.Remove(analyst);
                await _context.SaveChangesAsync();
            }
        }
    }
}
