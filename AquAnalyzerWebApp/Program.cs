using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;
using AquAnalyzerWebApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using AquAnalyzerWebApp.Auth;
using AquAnalyzerWebApp.Services;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI;
using AquAnalyzerAPI.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for API communication
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5044/") });

// Register application services
builder.Services.AddScoped<IAnalystService, AnalystService>();
builder.Services.AddScoped<IVisualDesignerService, VisualDesignerService>();
builder.Services.AddScoped<IAuthServiceAPI, AuthServiceAPI>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// Register DatabaseContext
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AuthorizationCore for Blazor's built-in authorization features
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

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


