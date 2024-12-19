using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Interfaces
{
    public interface IAbnormalityService
    {
        Task<Abnormality> AddAbnormality(Abnormality abnormality);
        Task<IEnumerable<Abnormality>> GetAllAbnormalities();
        Task<Abnormality> GetAbnormalityById(int id);
        Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type);
        Task<bool> MarkAbnormalityAsDealtWith(int abnormalityId);
        Task<bool> UpdateAbnormality(int id, string description, string type);
        Task<bool> DeleteAbnormality(int id);
        Task<IEnumerable<Abnormality>> CheckWaterDataAbnormalities(int dataId);
        Task<IEnumerable<Abnormality>> CheckWaterMetricsAbnormalities(int metricsId);
        Task RemoveDuplicateAbnormalities(int waterDataId);

    }
}