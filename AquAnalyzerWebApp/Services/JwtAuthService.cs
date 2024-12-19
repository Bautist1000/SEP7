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

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                Console.WriteLine($"Attempting login for user: {username}");

                var loginRequest = new
                {
                    Username = username,
                    Password = password
                };

                // Try analyst login
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync("auth/login-analyst", content);
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Analyst login response: {response.StatusCode}, Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    // Try visual designer login
                    response = await client.PostAsync("auth/login-visualdesigner", content);
                    responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Designer login response: {response.StatusCode}, Content: {responseContent}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                Jwt = responseContent;
                await CacheTokenAsync();

                ClaimsPrincipal principal = await CreateClaimsPrincipal();
                OnAuthStateChanged?.Invoke(principal);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
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
            try
            {
                Console.WriteLine($"Sending registration request for analyst: {analyst.Username}");
                string userAsJson = JsonSerializer.Serialize(new
                {
                    username = analyst.Username,
                    password = analyst.Password,
                    email = analyst.Email,
                    role = "Analyst"
                });

                StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

                // Update endpoint to match API controller route
                HttpResponseMessage response = await client.PostAsync("auth/register", content);
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Registration response: {response.StatusCode}, Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Registration failed: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterVisualDesignerAsync(VisualDesigner visualDesigner)
        {
            try
            {
                string userAsJson = JsonSerializer.Serialize(new
                {
                    username = visualDesigner.Username,
                    password = visualDesigner.Password,
                    email = visualDesigner.Email,
                    role = "VisualDesigner"
                });

                StringContent content = new(userAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("auth/register", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Registration response: {response.StatusCode}, Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Registration failed: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                throw;
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
    }
}
