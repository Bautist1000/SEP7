using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace AquAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterDataController : ControllerBase
    {
        private readonly IWaterDataService _waterDataService;

        public WaterDataController(IWaterDataService waterDataService)
        {
            _waterDataService = waterDataService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterData>> GetById(int id)
        {
            var data = await _waterDataService.GetWaterDataByIdAsync(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetAll()
        {
            var data = await _waterDataService.GetAllWaterDataAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<WaterData>> Add([FromBody] WaterData data)
        {
            if (data == null)
                return BadRequest("Water data is required.");

            await _waterDataService.AddWaterDataAsync(data);
            return CreatedAtAction(nameof(GetById), new { id = data.Id }, data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WaterData data)
        {
            if (id != data.Id)
                return BadRequest("Water data ID mismatch.");

            await _waterDataService.UpdateWaterDataAsync(data);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _waterDataService.DeleteWaterDataAsync(id);
            return NoContent();
        }

        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByLocation(string location)
        {
            var data = await _waterDataService.GetByLocationAsync(location);
            return Ok(data);
        }

        [HttpGet("abnormalities")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetWithAbnormalities()
        {
            var data = await _waterDataService.GetWithAbnormalitiesAsync();
            return Ok(data);
        }

        [HttpPost("generate-metrics")]
        public async Task<IActionResult> GenerateMetrics([FromBody] IEnumerable<WaterData> waterData)
        {
            if (waterData == null || !waterData.Any())
                return BadRequest("Water data cannot be null or empty");

            await _waterDataService.GenerateMetricsAsync(waterData);
            return Ok("Metrics generated successfully.");
        }

        [HttpGet("source-type/{sourceType}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetBySourceType(string sourceType)
        {
            var data = await _waterDataService.GetBySourceTypeAsync(sourceType);
            return Ok(data);
        }

        [HttpGet("usage-volume-range")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByUsageVolumeRange(double minVolume, double maxVolume)
        {
            var data = await _waterDataService.GetByUsageVolumeRangeAsync(minVolume, maxVolume);
            return Ok(data);
        }

        [HttpGet("flow-rate-range")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByFlowRateRange(double minFlowRate, double maxFlowRate)
        {
            var data = await _waterDataService.GetByFlowRateRangeAsync(minFlowRate, maxFlowRate);
            return Ok(data);
        }

        [HttpGet("electricity-consumption-range")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByElectricityConsumptionRange(double minConsumption, double maxConsumption)
        {
            var data = await _waterDataService.GetByElectricityConsumptionRangeAsync(minConsumption, maxConsumption);
            return Ok(data);
        }

        [HttpGet("product-id/{productId}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByProductId(double productId)
        {
            var data = await _waterDataService.GetByProductIdAsync(productId);
            return Ok(data);
        }

        [HttpGet("leak-detected/{leakDetected}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByLeakDetected(bool leakDetected)
        {
            var data = await _waterDataService.GetByLeakDetectedAsync(leakDetected);
            return Ok(data);
        }

        [HttpGet("clean-energy-usage/{usesCleanEnergy}")]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetByCleanEnergyUsage(bool usesCleanEnergy)
        {
            var data = await _waterDataService.GetByCleanEnergyUsageAsync(usesCleanEnergy);
            return Ok(data);
        }
    }
}
