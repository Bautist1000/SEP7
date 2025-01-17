using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using System.Security.Claims;

namespace AquAnalyzerAPI.Interfaces
{
    public interface IAuthServiceAPI
    {
        Task<User?> ValidateUserAsync(string username, string password);

        // Validate an Analyst's credentials
        Task<Analyst> ValidateAnalyst(string username, string password);

        // Validate a Visual Designer's credentials
        Task<VisualDesigner> ValidateVisualDesigner(string username, string password);

        // Register a new Analyst
        Task RegisterAnalystAsync(Analyst analyst);

        // Register a new Visual Designer
        Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner);

        // Retrieve the authenticated user's ClaimsPrincipal
        Task<ClaimsPrincipal> GetAuthAsync();

        // Log out the current user
        Task LogoutAsync();

        // Generate a new authentication token
        Task<string> GenerateTokenAsync(User user);

        // Event to notify changes in authentication state
        Action<ClaimsPrincipal>? OnAuthStateChanged { get; set; }
    }
}
