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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5126/") });
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IAbnormalityService, AbnormalityService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();