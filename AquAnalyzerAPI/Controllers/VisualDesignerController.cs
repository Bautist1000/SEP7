using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;

namespace AquAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisualDesignerController : ControllerBase
    {
        private readonly IVisualDesignerService _service;

        public VisualDesignerController(IVisualDesignerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualDesigner>>> GetAllVisDesig()
        {
            var visualDesigners = await _service.GetAllVisDesig();
            return Ok(visualDesigners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisualDesigner>> GetByIdOfVisDesig(int id)
        {
            var visualDesigner = await _service.GetByIdOfVisDesig(id);
            if (visualDesigner == null)
            {
                return NotFound();
            }
            return Ok(visualDesigner);
        }

        [HttpPost]
        public async Task<ActionResult<VisualDesigner>> AddVisDesig(VisualDesigner visualDesigner)
        {
            var createdVisualDesigner = await _service.AddVisDesig(visualDesigner);
            return CreatedAtAction(nameof(GetByIdOfVisDesig), new { id = createdVisualDesigner.Id }, createdVisualDesigner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisDesig(int id, VisualDesigner visualDesigner)
        {
            if (id != visualDesigner.Id)
            {
                return BadRequest();
            }

            await _service.UpdateVisDesig(visualDesigner);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisDesig(int id)
        {
            await _service.DeleteVisDesig(id);
            return NoContent();
        }
    }
}
