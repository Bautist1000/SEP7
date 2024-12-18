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

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<VisualisationData>>> SearchVisualisationsByType(string searchTerm)
        {
            var visualisations = await _visualisationService.SearchVisualisationsByType(searchTerm);
            return Ok(visualisations);
        }
    }

}
