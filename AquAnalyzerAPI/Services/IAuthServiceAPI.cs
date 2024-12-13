using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AquAnalyzerAPI.Services
{
    public interface IAuthServiceAPI
    {
        Task<Analyst> ValidateAnalyst(string username, string password);

        Task<VisualDesigner> ValidateVisualDesigner(string username, string password);

        // Register a new Analyst
        Task RegisterAnalystAsync(Analyst analyst);

        // Register a new Visual Designer
        Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner);

        // Retrieve the authenticated user's ClaimsPrincipal
        Task<ClaimsPrincipal> GetAuthAsync();

        // Log out the current user
        Task LogoutAsync();

        // Change password for an Analyst
        Task ChangeAnalystPasswordAsync(int id, string newPassword);

        // Change password for a Visual Designer
        Task ChangeVisualDesignerPasswordAsync(int id, string newPassword);

        // Generate a new authentication token
        Task<string> GenerateTokenAsync(User user);

        // Refresh an expired token
        Task<string> RefreshTokenAsync(string expiredToken);

        // Validate an authentication token
        Task<ClaimsPrincipal> ValidateTokenAsync(string token);

        // Event to notify changes in authentication state
        Action<ClaimsPrincipal>? OnAuthStateChanged { get; set; }
    }
}
