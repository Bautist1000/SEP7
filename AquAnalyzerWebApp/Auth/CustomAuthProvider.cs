using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using AquAnalyzerAPI.Interfaces; // Replace with your actual namespace

namespace AquAnalyzerWebApp.Auth
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IAuthServiceAPI authService;

        public CustomAuthProvider(IAuthServiceAPI authService)
        {
            this.authService = authService;
            authService.OnAuthStateChanged += AuthStateChanged; // Ensure your auth service has this event
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Fetch the ClaimsPrincipal for the authenticated user
            ClaimsPrincipal principal = await authService.GetAuthAsync();

            // Return the authentication state
            return new AuthenticationState(principal);
        }

        private void AuthStateChanged(ClaimsPrincipal principal)
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(principal))
            );
        }
    }
}
