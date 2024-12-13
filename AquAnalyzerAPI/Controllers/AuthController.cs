using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration config, IAuthServiceAPI authService) : ControllerBase
{
    private readonly IConfiguration _config = config;
    private readonly IAuthServiceAPI _authService = authService;

    [HttpPost("register")]
public async Task<ActionResult> Register([FromBody] User user)
{
    try
    {
        if (user.Role == "Analyst")
        {
            var analyst = new Analyst
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Role = "Analyst"
            };
            await _authService.RegisterAnalystAsync(analyst);
        }
        else if (user.Role == "VisualDesigner")
        {
            var visualDesigner = new VisualDesigner
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Role = "VisualDesigner"
            };
            await _authService.RegisterVisualDesignerAsync(visualDesigner);
        }
        else
        {
            return BadRequest("Invalid role.");
        }

        return Ok("Registration successful.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}




    [HttpPost("login-analyst")]
    public async Task<ActionResult> LoginAnalyst([FromBody] Analyst analyst)
    {
        try
        {
            // Validate Analyst credentials
            Analyst validatedAnalyst = await _authService.ValidateAnalyst(analyst.Username, analyst.Password);

            // Generate JWT
            string token = GenerateJwt(validatedAnalyst);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login-visualdesigner")]
    public async Task<ActionResult> LoginVisualDesigner([FromBody] VisualDesigner visualDesigner)
    {
        try
        {
            // Validate Visual Designer credentials
            VisualDesigner validatedDesigner = await _authService.ValidateVisualDesigner(visualDesigner.Username, visualDesigner.Password);

            // Generate JWT
            string token = GenerateJwt(validatedDesigner);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "");

        List<Claim> claims = GenerateClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private List<Claim> GenerateClaims(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"] ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Role, user is Analyst ? "Analyst" : "VisualDesigner"),
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("email", user.Email),
            new Claim("password", user.Password),
            new Claim("Role", user is Analyst ? "Analyst" : "VisualDesigner")
        };

        return claims.ToList();
    }
}
