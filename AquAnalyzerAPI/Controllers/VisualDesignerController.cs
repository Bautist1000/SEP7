using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisualDesignerController : ControllerBase
    {
        private readonly IVisualDesignerService _visualDesignerService;

        public VisualDesignerController(IVisualDesignerService service)
        {
            _visualDesignerService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualDesigner>>> GetAllVisDesig()
        {
            var visualDesigners = await _visualDesignerService.GetAllVisDesig();
            return Ok(visualDesigners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisualDesigner>> GetByIdOfVisDesig(int id)
        {
            var visualDesigner = await _visualDesignerService.GetByIdOfVisDesig(id);
            if (visualDesigner == null)
            {
                return NotFound();
            }
            return Ok(visualDesigner);
        }

        [HttpPost]
        public async Task<ActionResult<VisualDesigner>> AddVisDesig(VisualDesigner visualDesigner)
        {
            var createdVisualDesigner = await _visualDesignerService.AddVisDesig(visualDesigner);
            return CreatedAtAction(nameof(GetByIdOfVisDesig), new { id = createdVisualDesigner.Id }, createdVisualDesigner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisDesig(int id, VisualDesigner visualDesigner)
        {
            if (id != visualDesigner.Id)
            {
                return BadRequest();
            }

            await _visualDesignerService.UpdateVisDesig(visualDesigner);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisDesig(int id)
        {
            await _visualDesignerService.DeleteVisDesig(id);
            return NoContent();
        }
    }
}
