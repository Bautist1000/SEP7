using System.Security.Claims;
using System.Threading.Tasks;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Controllers;

namespace AquAnalyzerWebApp.Services{


public interface IAuthService
{
    // Login a user (Analyst or Visual Designer)
    Task<bool> LoginAsync(string username, string password);

    // Logout the current user
    Task LogoutAsync();

    // Event to notify about authentication state changes
    Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }

    // Retrieve the authenticated user's ClaimsPrincipal
    Task<ClaimsPrincipal> GetAuthAsync();

    // Register a new Analyst
    Task RegisterAnalystAsync(Analyst analyst);

    // Register a new Visual Designer
    Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner);
}
}

