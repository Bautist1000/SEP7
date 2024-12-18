using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using AquAnalyzerAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddScoped<IAnalystService, AnalystService>();
builder.Services.AddScoped<IVisualDesignerService, VisualDesignerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAbnormalityService, AbnormalityService>();
builder.Services.AddScoped<IWaterDataService, WaterDataService>();
builder.Services.AddScoped<IWaterMetricsService, WaterMetricsService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
        builder.WithOrigins("http://localhost:5065")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient"); // Apply CORS policy
app.UseAuthorization();

app.MapControllers();
app.Run();

