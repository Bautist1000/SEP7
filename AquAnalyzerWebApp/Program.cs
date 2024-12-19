using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AquAnalyzerAPI.Files;
using AquAnalyzerWebApp.Services;
using AquAnalyzerWebApp.Interfaces;
using AquAnalyzerWebApp.Components;
using AquAnalyzerWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5126/") });


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddHttpClient<INotificationsService, NotificationsService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5126/");
    client.Timeout = TimeSpan.FromSeconds(30);
});


// Register Authentication State Provider for Blazor
// builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// // Add AuthorizationCore and policies
// builder.Services.AddAuthorizationCore();
// AuthorizationPolicies.AddPolicies(builder.Services);

// Configure Authentication (Single Registration)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/login";
});

// Configure the HTTP request pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();