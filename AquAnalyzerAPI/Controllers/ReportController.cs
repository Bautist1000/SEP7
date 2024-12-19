using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using AquAnalyzerAPI.Dtos;
namespace AquAnalyzerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
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
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            try
            {
                var report = await _reportService.GetReportById(id);
                if (report == null) return NotFound();
                return Ok(report);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> AddReport(Report report)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdReport = await _reportService.AddReport(report);
                return CreatedAtAction(nameof(GetReportById),
                    new { id = createdReport.Id }, createdReport);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating report", ex);
                return StatusCode(500, "Internal server error while creating report");
            }
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
        public async Task<ActionResult<IEnumerable<ReportDto>>> SearchReportsByTitle(string searchTerm)
        {
            var reports = await _reportService.SearchReportsByTitle(searchTerm);
            return Ok(reports);
        }
    }

}
