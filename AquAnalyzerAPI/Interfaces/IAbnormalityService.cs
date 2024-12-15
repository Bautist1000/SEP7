using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IAbnormalityService
    {
        Task<Abnormality> AddAbnormality(Abnormality abnormality);
        Task<IEnumerable<Abnormality>> GetAllAbnormalities();
        Task<Abnormality> GetAbnormalityById(int id);
        Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type);
        Task<bool> MarkAsDealtWith(int id);
        Task<bool> UpdateAbnormality(int id, string description, string type);
        Task<bool> DeleteAbnormality(int id);
        Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId);
        Task<IEnumerable<Abnormality>> CheckWaterMetricsAbnormalities(int metricsId);
    }
}