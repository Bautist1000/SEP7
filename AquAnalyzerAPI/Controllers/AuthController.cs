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

    [HttpPost("register-analyst")]
public async Task<ActionResult> RegisterAnalyst([FromBody] Analyst analyst)
{
    try
    {
        await _authService.RegisterAnalystAsync(analyst);
        return Ok("Registration successful.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

[HttpPost("register-VisualDesigner")]
public async Task<ActionResult> RegisterVisualDesigner([FromBody] VisualDesigner visualDesigner)
{
    try
    {
        await _authService.RegisterVisualDesignerAsync(visualDesigner);
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
            Analyst validatedAnalyst = await _authService.ValidateAnalyst(analyst.Id, analyst.Password);

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
            VisualDesigner validatedDesigner = await _authService.ValidateVisualDesigner(visualDesigner.Id, visualDesigner.Password);

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
            new Claim("Role", user is Analyst ? "Analyst" : "VisualDesigner")
        };

        return claims.ToList();
    }
}
