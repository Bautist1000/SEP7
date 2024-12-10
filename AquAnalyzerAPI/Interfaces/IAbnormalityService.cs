using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IAbnormalityService
    {
        Task<Abnormality> AddAbnormality(Abnormality abnormality);
        Task<IEnumerable<Abnormality>> GetAllAbnormalities();
        Task<Abnormality> GetAbnormalityById(int id);
        Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type);
        Task<bool> UpdateAbnormality(int id, string description, string type);
        Task<bool> DeleteAbnormality(int id);
    }
}