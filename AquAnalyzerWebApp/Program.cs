using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using AquAnalyzerAPI.Files;
using AquAnalyzerWebApp.Services;
using AquAnalyzerWebApp.Interfaces;
using AquAnalyzerWebApp.Components;
using AquAnalyzerWebApp.Auth;
using Radzen;
var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiServer", policy =>
        policy.WithOrigins("http://localhost:5126")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Add Blazor services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Authentication & Authorization first
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddAuthorizationCore();

// Then add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAnalyst", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "Analyst"));

    options.AddPolicy("MustBeVisualDesigner", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "VisualDesigner"));
});

// Register application services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IVisualisationPageService, VisualisationPageService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();

// Add HTTP client
builder.Services.AddHttpClient<INotificationsService, NotificationsService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5126/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors("AllowApiServer");
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery(); // Add this line
app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();