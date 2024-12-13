using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services; // Assuming you have implementations for these interfaces
using AquAnalyzerWebApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using AquAnalyzerWebApp.Auth;
using AquAnalyzerAPI.Files; // Assuming DatabaseContext is defined here
using AquAnalyzerAPI;



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

// Optional: Add Cookie Authentication if needed
builder.Services.AddAuthentication().AddCookie(options =>
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
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

