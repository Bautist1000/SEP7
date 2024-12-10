using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWaterDataService
{
    Task<WaterData> GetByIdAsync(int id);
    Task<IEnumerable<WaterData>> GetAllAsync();
    Task AddWaterDataAsync(WaterData data);
    Task UpdateWaterDataAsync(WaterData data);
    Task DeleteWaterDataAsync(int id);
    Task<IEnumerable<WaterData>> GetByLocationAsync(string location);
    Task<IEnumerable<WaterData>> GetWithAbnormalitiesAsync();
    Task GenerateMetricsAsync(IEnumerable<WaterData> waterData);
    Task<IEnumerable<WaterData>> GetBySourceTypeAsync(string sourceType);
    Task<IEnumerable<WaterData>> GetByUsageVolumeRangeAsync(double minVolume, double maxVolume);
    Task<IEnumerable<WaterData>> GetByFlowRateRangeAsync(double minFlowRate, double maxFlowRate);
    Task<IEnumerable<WaterData>> GetByElectricityConsumptionRangeAsync(double minConsumption, double maxConsumption);
    Task<IEnumerable<WaterData>> GetByProductIdAsync(double productId);
    Task<IEnumerable<WaterData>> GetByLeakDetectedAsync(bool leakDetected);
    Task<IEnumerable<WaterData>> GetByCleanEnergyUsageAsync(bool usesCleanEnergy);
}