
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
        private readonly DatabaseContext _context;
        private ClaimsPrincipal _currentPrincipal = new ClaimsPrincipal();

        public AuthServiceAPI(DatabaseContext context)
        {
            _context = context;
        }

        // Event to notify about authentication state changes
        public Action<ClaimsPrincipal>? OnAuthStateChanged { get; set; }

        // Register a new Analyst
        public async Task RegisterAnalystAsync(Analyst analyst)
        {
            if (_context.Users.Any(u => u.Username == analyst.Username || u.Email == analyst.Email))
            {
                throw new Exception("Username or email is already in use.");
            }

            _context.Analysts.Add(analyst);
            await _context.SaveChangesAsync();
        }

        // Register a new Visual Designer
        public async Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
        {
            if (_context.Users.Any(u => u.Username == visualDesigner.Username || u.Email == visualDesigner.Email))
            {
                throw new Exception("Username or email is already in use.");
            }

            _context.VisualDesigners.Add(visualDesigner);
            await _context.SaveChangesAsync();
        }

        // Validate an Analyst's credentials
        public async Task<Analyst> ValidateAnalyst(string username, string password)
        {
            var analyst = _context.Analysts.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (analyst == null)
            {
                throw new Exception("Invalid username or password.");
            }
            return analyst;
        }

        // Validate a Visual Designer's credentials
        public async Task<VisualDesigner> ValidateVisualDesigner(string username, string password)
        {
            var visualDesigner = _context.VisualDesigners.FirstOrDefault(v => v.Username == username && v.Password == password);
            if (visualDesigner == null)
            {
                throw new Exception("Invalid username or password.");
            }
            return visualDesigner;
        }

        // Generate a JWT token
        public Task<string> GenerateTokenAsync(User user)
        {
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

        // Get the current authenticated user
        public Task<ClaimsPrincipal> GetAuthAsync()
        {
            return Task.FromResult(_currentPrincipal);
        }

        // Logout the current user
        public Task LogoutAsync()
        {
            _currentPrincipal = new ClaimsPrincipal();
            OnAuthStateChanged?.Invoke(_currentPrincipal);
            return Task.CompletedTask;
        }

        // Not yet implemented methods (optional to implement)
        public Task ChangeAnalystPasswordAsync(int id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task ChangeVisualDesignerPasswordAsync(int id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshTokenAsync(string expiredToken)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
