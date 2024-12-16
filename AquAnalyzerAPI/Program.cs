using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this if missing


// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAnalystService, AnalystService>();
builder.Services.AddScoped<IVisualDesignerService, VisualDesignerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAbnormalityService, AbnormalityService>();
builder.Services.AddScoped<IWaterDataService, WaterDataService>();
builder.Services.AddScoped<IWaterMetricsService, WaterMetricsService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();