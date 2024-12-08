public class ReportService(DatabaseContext context) : IReportService
{
    private readonly DatabaseContext _context = context;

    // Add a new report
    public async Task<Report> AddReport(Report report)
    {
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
        return report;
    }

    // Get a report by its ID
    public async Task<Report> GetReportById(int id)
    {
        return await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
    }

    // Get all reports
    public async Task<IEnumerable<Report>> GetAllReports()
    {
        return await _context.Reports.ToListAsync();
    }

    // Update a report
    public async Task UpdateReport(Report updatedReport)
    {
        var reportToUpdate = await _context.Reports.FirstOrDefaultAsync(r => r.Id == updatedReport.Id);
        if (reportToUpdate != null)
        {
            reportToUpdate.Title = updatedReport.Title;
            reportToUpdate.Description = updatedReport.Description;
            reportToUpdate.UserId = updatedReport.UserId;
            reportToUpdate.Visualisations = updatedReport.Visualisations;
            await _context.SaveChangesAsync();
        }
    }

    // Delete a report
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

    // Search reports by title
    public async Task<IEnumerable<Report>> SearchReportsByTitle(string searchTerm)
    {
        return await _context.Reports
            .Where(r => r.Title.ToLower().Contains(searchTerm.ToLower()))
            .ToListAsync();
    }
}
