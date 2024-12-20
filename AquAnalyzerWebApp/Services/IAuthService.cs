using System.Security.Claims;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Controllers;

namespace AquAnalyzerWebApp.Services
{


    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);

        Task LogoutAsync();

        Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }

        Task<ClaimsPrincipal> GetAuthAsync();

        Task RegisterAnalystAsync(Analyst analyst);

        Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner);
    }
}