using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;

namespace AquAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalystController : ControllerBase
    {
        private readonly IAnalystService _service;

        public AnalystController(IAnalystService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Analyst>>> GetAllAnalysts()
        {
            var analysts = await _service.GetAllAnalysts();
            return Ok(analysts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Analyst>> GetAnalystById(int id)
        {
            var analyst = await _service.GetAnalystById(id);
            if (analyst == null)
            {
                return NotFound();
            }
            return Ok(analyst);
        }

        [HttpPost]
        public async Task<ActionResult<Analyst>> AddAnalyst(Analyst analyst)
        {
            var createdAnalyst = await _service.AddAnalyst(analyst);
            return CreatedAtAction(nameof(GetAnalystById), new { id = createdAnalyst.Id }, createdAnalyst);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnalyst(int id, Analyst analyst)
        {
            if (id != analyst.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAnalyst(analyst);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalyst(int id)
        {
            await _service.DeleteAnalyst(id);
            return NoContent();
        }
    }
}
