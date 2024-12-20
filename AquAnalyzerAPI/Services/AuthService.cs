
using System.Security.Claims;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AquAnalyzerAPI.Services
{
    public class AuthServiceAPI : IAuthServiceAPI
    {
        private readonly DatabaseContext _context;
        private ClaimsPrincipal _currentPrincipal = new ClaimsPrincipal();
        private readonly IConfiguration _config;
        public AuthServiceAPI(DatabaseContext context, IConfiguration config)
        {
            _config = config;

            _context = context;
        }

        // Event to notify about authentication state changes
        public Action<ClaimsPrincipal>? OnAuthStateChanged { get; set; }

        // Register a new Analyst
        public async Task RegisterAnalystAsync(Analyst analyst)
        {
            if (await UserExistsAsync(analyst.Username, analyst.Email))
            {
                throw new Exception("Username or email is already in use.");
            }

            _context.Analysts.Add(analyst);
            await _context.SaveChangesAsync();
        }

        // Register a new Visual Designer
        public async Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
        {
            if (await UserExistsAsync(visualDesigner.Username, visualDesigner.Email))
            {
                throw new Exception("Username or email is already in use.");
            }

            _context.VisualDesigners.Add(visualDesigner);
            await _context.SaveChangesAsync();
        }

        // Validate a Visual Designer's credentials
        public async Task<VisualDesigner> ValidateVisualDesigner(string username, string password)
        {
            var visualDesigner = await _context.VisualDesigners.FirstOrDefaultAsync(v => v.Username == username && v.Password == password);
            if (visualDesigner == null)
            {
                throw new Exception("Invalid username or password.");
            }

            return visualDesigner;
        }

        // Validate an Analyst's credentials
        public async Task<Analyst> ValidateAnalyst(string username, string password)
        {
            var analyst = await _context.Analysts.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
            if (analyst == null)
            {
                throw new Exception("Invalid username or password.");
            }

            return analyst;
        }

        // Validate a user's credentials (either Analyst or Visual Designer)
        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            // Search in Analysts table
            var analyst = await _context.Analysts.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
            if (analyst != null)
            {
                return analyst;
            }

            // If not found, search in Visual Designers table
            var visualDesigner = await _context.VisualDesigners.FirstOrDefaultAsync(v => v.Username == username && v.Password == password);
            if (visualDesigner != null)
            {
                return visualDesigner;
            }

            // If neither found, return null
            throw new Exception("Invalid username or password.");
        }

        // Generate a JWT token
        public Task<string> GenerateTokenAsync(User user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ??
                "MypussytasteslikePepsicolaMyeyesarewidelikecherrypiesIgotsweettasteformenwhoareolderIt'salwaysbeenso,it'snosurprise"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
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

        // Helper to check if a username or email is already in use
        private async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Analysts.AnyAsync(a => a.Username == username || a.Email == email) ||
                   await _context.VisualDesigners.AnyAsync(v => v.Username == username || v.Email == email);
        }

        // Not yet implemented methods
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
