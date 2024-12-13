using System.Security.Claims;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AquAnalyzerAPI.Services
{
    public class AuthServiceAPI : IAuthServiceAPI
    {
        private ClaimsPrincipal _currentPrincipal = new ClaimsPrincipal();

        // Implement the OnAuthStateChanged property
        public Action<ClaimsPrincipal>? OnAuthStateChanged { get; set; }

        public Task<Analyst> ValidateAnalyst(int id, string password)
        {
            // Logic to validate an Analyst
            throw new NotImplementedException();
        }

        public Task<VisualDesigner> ValidateVisualDesigner(int id, string password)
        {
            // Logic to validate a Visual Designer
            throw new NotImplementedException();
        }

        public Task RegisterAnalystAsync(Analyst analyst)
        {
            // Logic to register a new Analyst
            throw new NotImplementedException();
        }

        public Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
        {
            // Logic to register a new Visual Designer
            throw new NotImplementedException();
        }

        public Task<ClaimsPrincipal> GetAuthAsync()
        {
            // Return the current authenticated principal
            return Task.FromResult(_currentPrincipal);
        }

        public Task LogoutAsync()
        {
            // Clear the current principal and notify subscribers
            _currentPrincipal = new ClaimsPrincipal();
            OnAuthStateChanged?.Invoke(_currentPrincipal);
            return Task.CompletedTask;
        }

        public Task ChangeAnalystPasswordAsync(int id, string newPassword)
        {
            // Logic to change password for an Analyst
            throw new NotImplementedException();
        }

        public Task ChangeVisualDesignerPasswordAsync(int id, string newPassword)
        {
            // Logic to change password for a Visual Designer
            throw new NotImplementedException();
        }

        public Task<string> GenerateTokenAsync(User user)
        {
            // Generate a JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey123"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> RefreshTokenAsync(string expiredToken)
        {
            // Logic to refresh an expired token
            throw new NotImplementedException();
        }

        public Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            // Logic to validate an authentication token
            throw new NotImplementedException();
        }
    }
}
