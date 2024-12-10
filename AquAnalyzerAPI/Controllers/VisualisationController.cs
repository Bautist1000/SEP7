using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
namespace AquAnalyzerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VisualisationController(IVisualisationService _visualisationService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visualisation>>> GetAllVisualisations()
        {
            var visualisations = await _visualisationService.GetAllVisualisations();
            return Ok(visualisations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visualisation>> GetVisualisationById(int id)
        {
            var visualisation = await _visualisationService.GetVisualisationById(id);
            if (visualisation == null) return NotFound();
            return Ok(visualisation);
        }

        [HttpPost]
        public async Task<ActionResult<Visualisation>> AddVisualisation(Visualisation visualisation)
        {
            await _visualisationService.AddVisualisation(visualisation);
            return CreatedAtAction(nameof(GetVisualisationById), new { id = visualisation.Id }, visualisation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVisualisation(int id, Visualisation visualisation)
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
        public async Task<ActionResult<IEnumerable<Visualisation>>> SearchVisualisationsByType(string searchTerm)
        {
            var visualisations = await _visualisationService.SearchVisualisationsByType(searchTerm);
            return Ok(visualisations);
        }
    }

}
