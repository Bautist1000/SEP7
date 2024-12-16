using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReportController(IReportService _reportService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetAllReports()
        {
            try
            {
                var reports = await _reportService.GetAllReports();
                return Ok(reports);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReportById(int id)
        {
            var report = await _reportService.GetReportById(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        [HttpPost]
        public async Task<ActionResult<Report>> AddReport(Report report)
        {
            await _reportService.AddReport(report);
            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReport(int id, Report report)
        {
            if (id != report.Id) return BadRequest();
            await _reportService.UpdateReport(report);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(int id)
        {
            var success = await _reportService.DeleteReport(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<Report>>> SearchReportsByTitle(string searchTerm)
        {
            var reports = await _reportService.SearchReportsByTitle(searchTerm);
            return Ok(reports);
        }
    }

}
