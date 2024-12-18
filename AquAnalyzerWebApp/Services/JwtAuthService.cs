using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using AquAnalyzerAPI.Services;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerWebApp.Services
{

public class JwtAuthService(HttpClient client, IJSRuntime jsRuntime) : IAuthService
{
    public string Jwt { get; private set; } = "";
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;

    public async Task LoginAsync(string username, string password)
{
    string userAsJson = JsonSerializer.Serialize(new { username, password });
    StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("/auth/login", content);
    string responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception(responseContent);
    }

    Jwt = responseContent;
    await CacheTokenAsync();

    ClaimsPrincipal principal = await CreateClaimsPrincipal();
    OnAuthStateChanged.Invoke(principal);
}
    public async Task LogoutAsync()
    {
        await ClearTokenFromCacheAsync();
        Jwt = "";
        ClaimsPrincipal principal = new();
        OnAuthStateChanged.Invoke(principal);
    }

    public async Task<ClaimsPrincipal> GetAuthAsync()
    {
        return await CreateClaimsPrincipal();
    }

    public async Task RegisterAnalystAsync(Analyst analyst)
    {
        string userAsJson = JsonSerializer.Serialize(analyst);
        StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("auth/register-analyst", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
    {
        string userAsJson = JsonSerializer.Serialize(visualDesigner);
        StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("auth/register-visualdesigner", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    private async Task<ClaimsPrincipal> CreateClaimsPrincipal()
    {
        var cachedToken = await GetTokenFromCacheAsync();
        if (string.IsNullOrEmpty(Jwt) && string.IsNullOrEmpty(cachedToken))
        {
            return new ClaimsPrincipal();
        }
        if (!string.IsNullOrEmpty(cachedToken))
        {
            Jwt = cachedToken;
        }
        if (!client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Jwt);
        }

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes) 
                     ?? new Dictionary<string, object>();
        return claims.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private async Task<string?> GetTokenFromCacheAsync()
    {
        return await jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
    }

    private async Task CacheTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", Jwt);
    }

    private async Task ClearTokenFromCacheAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", "");
    }
} }
