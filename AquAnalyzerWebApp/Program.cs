using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Files;
using AquAnalyzerWebApp.Services;
using AquAnalyzerWebApp.Interfaces;
using AquAnalyzerWebApp.Components;
using AquAnalyzerWebApp.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5126/") });
builder.Services.AddRadzenComponents();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IVisualisationPageService, VisualisationPageService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddHttpClient<INotificationsService, NotificationsService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5126/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/login";
});

// Add Authentication State Provider for Blazor
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// Add AuthorizationCore and policies
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("MustBeAnalyst", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "Analyst"));

    options.AddPolicy("MustBeVisualDesigner", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "VisualDesigner"));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiServer", policy =>
        policy.WithOrigins("http://localhost:5126")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseCors("AllowApiServer");
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
