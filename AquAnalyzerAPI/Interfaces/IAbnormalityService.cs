

public interface IAbnormalityService
{
    Task<IEnumerable<Abnormality>> GetAllAbnormalities();
    Task<Abnormality> GetAbnormalityById(int id);
    Task<IEnumerable<Abnormality>> GetAbnormalitiesByType(string type);
    Task<Abnormality> AddAbnormality(Abnormality abnormality);
    Task<bool> UpdateAbnormality(int id, string description, string type);
    Task<bool> DeleteAbnormality(int id);
}
