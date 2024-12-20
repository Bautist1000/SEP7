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
using Radzen.Blazor;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5126/") });
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// Register application services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IVisualisationPageService, VisualisationPageService>();
builder.Services.AddScoped<IReportPageService, ReportPageService>();
builder.Services.AddHttpClient<INotificationsService, NotificationsService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5126/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add Authentication Services
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddAuthorizationCore(options =>
{
    // Policy for Analysts
    options.AddPolicy("MustBeAnalyst", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "Analyst"));

    // Policy for Visual Designers
    options.AddPolicy("MustBeVisualDesigner", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim(ClaimTypes.Role, "VisualDesigner"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiServer", policy =>
        policy.WithOrigins("http://localhost:5126")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Configure JWT authentication parameters
builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
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
});

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();


// Configure the HTTP request pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("AllowApiServer");
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();