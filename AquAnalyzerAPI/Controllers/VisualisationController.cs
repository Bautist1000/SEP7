using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisualisationController(IVisualisationService _visualisationService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualisationData>>> GetAllVisualisations()
        {
            var visualisations = await _visualisationService.GetAllVisualisations();
            return Ok(visualisations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisualisationData>> GetVisualisationById(int id)
        {
            var visualisation = await _visualisationService.GetVisualisationById(id);
            if (visualisation == null) return NotFound();
            return Ok(visualisation);
        }

        [HttpPost]
        public async Task<ActionResult<VisualisationData>> AddVisualisation(VisualisationData visualisation)
        {
            await _visualisationService.AddVisualisation(visualisation);
            return CreatedAtAction(nameof(GetVisualisationById), new { id = visualisation.Id }, visualisation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVisualisation(int id, VisualisationData visualisation)
        {
            if (id != visualisation.Id) return BadRequest();
            await _visualisationService.UpdateVisualisation(visualisation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVisualisation(int id)
        {
            var success = await _visualisationService.DeleteVisualisation(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/waterdata/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetWaterDataForChart(int id, string startDate, string endDate)
        {
            try
            {
                DateTime? start = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : null;
                DateTime? end = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : null;

                var data = await _visualisationService.GetWaterDataForChart(id, start, end);
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving water data");
            }
        }

        [HttpGet("{id}/metrics/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<WaterMetrics>>> GetMetricsForChart(int id, string startDate, string endDate)
        {
            try
            {
                DateTime? start = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : null;
                DateTime? end = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : null;

                var metrics = await _visualisationService.GetMetricsForChart(id, start, end);
                return Ok(metrics);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving metrics data");
            }
        }

        [HttpPut("{id}/charttype")]
        public async Task<ActionResult> UpdateChartType(int id, string chartType)
        {
            try
            {
                await _visualisationService.UpdateChartType(id, chartType);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating chart type");
            }
        }
    }
}
