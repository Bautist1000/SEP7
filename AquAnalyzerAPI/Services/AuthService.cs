using System.Security.Claims;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Files;

namespace AquAnalyzerAPI.Services
{
    public class AuthServiceAPI : IAuthServiceAPI
    {
        private readonly DatabaseContext _context;

        public AuthServiceAPI(DatabaseContext context)
        {
            _context = context;
        }

        // Validate an Analyst by ID and Password
        public Task<Analyst> ValidateAnalyst(int id, string password)
        {
            var existingAnalyst = _context.Analysts.FirstOrDefault(a => a.Id == id)
                ?? throw new Exception("Analyst not found");

            if (!existingAnalyst.Password.Equals(password))
            {
                throw new Exception("Password mismatch");
            }

            return Task.FromResult(existingAnalyst);
        }

        // Validate a Visual Designer by ID and Password
        public Task<VisualDesigner> ValidateVisualDesigner(int id, string password)
        {
            var existingVisualDesigner = _context.VisualDesigners.FirstOrDefault(v => v.Id == id)
                ?? throw new Exception("Visual Designer not found");

            if (!existingVisualDesigner.Password.Equals(password))
            {
                throw new Exception("Password mismatch");
            }

            return Task.FromResult(existingVisualDesigner);
        }

        // Register a new Analyst
        public async Task RegisterAnalystAsync(Analyst analyst)
        {
            _context.Analysts.Add(analyst);
            await _context.SaveChangesAsync();
        }

        // Register a new Visual Designer
        public async Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
        {
            _context.VisualDesigners.Add(visualDesigner);
            await _context.SaveChangesAsync();
        }

        // Retrieve the authenticated user's ClaimsPrincipal
        public Task<ClaimsPrincipal> GetAuthAsync()
        {
            // Example: Fetch current user information and convert to ClaimsPrincipal
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Role, "Analyst") // Example: Role fetched from context
            }, "api");

            var principal = new ClaimsPrincipal(identity);
            return Task.FromResult(principal);
        }

        // Log out the current user
        public Task LogoutAsync()
        {
            // Implement logic to log out the user (if needed, e.g., clearing session data)
            return Task.CompletedTask;
        }

        // Change password for an Analyst
        public async Task ChangeAnalystPasswordAsync(int id, string newPassword)
        {
            var analyst = _context.Analysts.FirstOrDefault(a => a.Id == id)
                ?? throw new Exception("Analyst not found");

            analyst.Password = newPassword;
            await _context.SaveChangesAsync();
        }

        // Change password for a Visual Designer
        public async Task ChangeVisualDesignerPasswordAsync(int id, string newPassword)
        {
            var visualDesigner = _context.VisualDesigners.FirstOrDefault(v => v.Id == id)
                ?? throw new Exception("Visual Designer not found");

            visualDesigner.Password = newPassword;
            await _context.SaveChangesAsync();
        }

        // Generate a new authentication token
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

        // Refresh an expired token
        public Task<string> RefreshTokenAsync(string expiredToken)
        {
            // Implement token refresh logic here
            throw new NotImplementedException();
        }

        // Validate an authentication token
        public Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            // Example token validation (simplified for illustration)
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ReadJwtToken(token).Claims;

            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }
    }
}
