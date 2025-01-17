using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerWebApp.Services;

namespace AquAnalyzerWebApp.Auth
{
    public class CustomAuthProvider : AuthenticationStateProvider, IDisposable
    {
        private readonly IAuthService _authService;
        private bool _initialized;
        private bool _disposed;

        public CustomAuthProvider(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _authService.OnAuthStateChanged += AuthStateChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_initialized)
            {
                _initialized = true;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var principal = await _authService.GetAuthAsync();
                return new AuthenticationState(principal ?? new ClaimsPrincipal(new ClaimsIdentity()));
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private void AuthStateChanged(ClaimsPrincipal principal)
        {
            if (!_disposed)
            {
                NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(principal))
                );
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _authService.OnAuthStateChanged -= AuthStateChanged;
            }
        }
    }
}