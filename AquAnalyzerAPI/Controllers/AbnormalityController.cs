using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquAnalyzerAPI.Interfaces;


[Route("api/[controller]")]
[ApiController]
public class AbnormalityController : ControllerBase
{
    private readonly IAbnormalityService _abnormalityService;

    public AbnormalityController(IAbnormalityService abnormalityService)
    {
        _abnormalityService = abnormalityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Abnormality>>> GetAllAbnormalities()
    {
        return Ok(await _abnormalityService.GetAllAbnormalities());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Abnormality>> GetAbnormalityById(int id)
    {
        var abnormality = await _abnormalityService.GetAbnormalityById(id);
        if (abnormality == null) return NotFound();

        return Ok(abnormality);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Abnormality>>> GetAbnormalitiesByType(string type)
    {
        var abnormalities = await _abnormalityService.GetAbnormalitiesByType(type);
        if (abnormalities == null) return NotFound();

        return Ok(abnormalities);
    }

    [HttpPost]
    public async Task<ActionResult<Abnormality>> AddAbnormality(Abnormality abnormality)
    {
        await _abnormalityService.AddAbnormality(abnormality);
        return CreatedAtAction(nameof(GetAbnormalityById), new { id = abnormality.Id }, abnormality);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAbnormality(int id, Abnormality updatedAbnormality)
    {
        var success = await _abnormalityService.UpdateAbnormality(id, updatedAbnormality.Description, updatedAbnormality.Type);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAbnormality(int id)
    {
        var success = await _abnormalityService.DeleteAbnormality(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
