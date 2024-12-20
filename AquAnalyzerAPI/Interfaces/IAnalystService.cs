using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace AquAnalyzerAPI.Interfaces
{
    public interface IAnalystService
    {
        Task<IEnumerable<Analyst>> GetAllAnalysts();
        Task<Analyst> GetAnalystById(int id);
        Task<Analyst> AddAnalyst(Analyst analyst);
        Task<Analyst> UpdateAnalyst(Analyst analyst);
        Task DeleteAnalyst(int id);
    }
}
