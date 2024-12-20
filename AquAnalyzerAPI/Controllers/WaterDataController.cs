using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquAnalyzerAPI.Interfaces;
using System;
using System.Linq;

namespace AquAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterDataController : ControllerBase
    {
        private readonly IWaterDataService _waterDataService;

        public WaterDataController(IWaterDataService waterDataService)
        {
            _waterDataService = waterDataService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterData>> GetWaterDataById(int id)
        {
            var waterData = await _waterDataService.GetWaterDataByIdAsync(id);
            if (waterData == null)
            {
                return NotFound();
            }
            return Ok(waterData);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterData>>> GetAllWaterData()
        {
            var waterData = await _waterDataService.GetAllWaterDataAsync();
            return Ok(waterData);
        }

        [HttpPost]
        public async Task<ActionResult> AddWaterData(WaterData waterData)
        {
            try
            {
                if (waterData.WaterMetrics == null)
                {
                    waterData.WaterMetrics = new WaterMetrics
                    {
                        DateGeneratedOn = DateTime.UtcNow
                    };
                }

                await _waterDataService.AddWaterDataAsync(waterData);

                var abnormalityService = HttpContext.RequestServices.GetRequiredService<IAbnormalityService>();
                var abnormalities = await abnormalityService.CheckWaterDataAbnormalities(waterData.Id);

                if (abnormalities.Any())
                {
                    waterData.HasAbnormalities = true;
                    await _waterDataService.UpdateWaterDataAsync(waterData);

                    foreach (var abnormality in abnormalities)
                    {
                        await abnormalityService.AddAbnormality(abnormality);
                    }
                }

                return CreatedAtAction(nameof(GetWaterDataById), new { id = waterData.Id }, waterData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaterData(int id, WaterData waterData)
        {
            try
            {
                if (id != waterData.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (waterData == null)
                {
                    return BadRequest("Water data cannot be null");
                }

                await _waterDataService.UpdateWaterDataAsync(waterData);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaterData(int id)
        {
            try
            {
                await _waterDataService.DeleteWaterDataAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
