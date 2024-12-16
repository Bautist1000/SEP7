using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace AquAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterMetricsController : ControllerBase
    {
        private readonly IWaterMetricsService _waterMetricsService;

        public WaterMetricsController(IWaterMetricsService waterMetricsService)
        {
            _waterMetricsService = waterMetricsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMetrics()
        {
            var metrics = await _waterMetricsService.GetAllMetricsAsync();
            return Ok(metrics);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMetricsById(int id)
        {
            var metrics = await _waterMetricsService.GetMetricsByIdAsync(id);
            if (metrics == null)
                return NotFound();
            return Ok(metrics);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMetrics([FromBody] IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                return BadRequest("Water data cannot be null or empty");

            await _waterMetricsService.GenerateMetricsAsync(waterData);
            return Ok("Metrics generated successfully.");
        }

        [HttpGet("average-flow-rate")]
        public async Task<IActionResult> GetAverageFlowRate([FromBody] IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                return BadRequest("Water data cannot be null or empty");

            var result = await _waterMetricsService.CalculateAverageFlowRateAsync(waterData);
            return Ok(result);
        }

        [HttpGet("count-abnormalities")]
        public async Task<IActionResult> GetCountAbnormalities([FromBody] IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                return BadRequest("Water data cannot be null or empty");

            var result = await _waterMetricsService.CountAbnormalitiesAsync(waterData);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddMetrics([FromBody] WaterMetrics metrics)
        {
            await _waterMetricsService.AddMetricsAsync(metrics);
            return CreatedAtAction(nameof(GetMetricsById), new { id = metrics.Id }, metrics);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMetrics(int id, [FromBody] WaterMetrics metrics)
        {
            if (id != metrics.Id)
                return BadRequest("Metric ID mismatch");

            await _waterMetricsService.UpdateMetricsAsync(metrics);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetrics(int id)
        {
            await _waterMetricsService.DeleteMetricsAsync(id);
            return NoContent();
        }
    }

}

