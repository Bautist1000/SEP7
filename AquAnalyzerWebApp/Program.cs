using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;
using AquAnalyzerWebApp.Components;
using AquAnalyzerWebApp.Services;
using AquAnalyzerWebApp.Interfaces;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Auth;
using AquAnalyzerWebApp.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5126/")
});
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IWaterService, WaterService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IAnalystService, AnalystService>();
builder.Services.AddScoped<IVisualDesignerService, VisualDesignerService>();
builder.Services.AddScoped<IAuthServiceAPI, AuthServiceAPI>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();

// Register Authentication State Provider for Blazor
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// Add AuthorizationCore and policies
builder.Services.AddAuthorizationCore();
AuthorizationPolicies.AddPolicies(builder.Services);

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();