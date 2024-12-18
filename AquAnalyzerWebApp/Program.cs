
using AquAnalyzerWebApp.Components;
using AquAnalyzerWebApp.Services;
using AquAnalyzerWebApp.Interfaces;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();