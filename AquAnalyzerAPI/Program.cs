using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Auth;
using AquAnalyzerAPI.Files;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this if missing


// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });




// Register application services
builder.Services.AddScoped<IAnalystService, AnalystService>();
builder.Services.AddScoped<IVisualDesignerService, VisualDesignerService>();
builder.Services.AddScoped<IAuthServiceAPI, AuthServiceAPI>();
builder.Services.AddScoped<IAbnormalityService, AbnormalityService>();
builder.Services.AddScoped<IWaterDataService, WaterDataService>();
builder.Services.AddScoped<IWaterMetricsService, WaterMetricsService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=database.db"));

// Add database context

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=database.db"));

builder.Services.AddScoped<IAuthServiceAPI, AuthServiceAPI>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AquAnalyzer API",
        Version = "v1",
        Description = "API for AquAnalyzer application",
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add JWT-based authentication directly
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero,
    };
});

// Add authorization policies
AuthorizationPolicies.AddPolicies(builder.Services);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
        policy.WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(';') ?? Array.Empty<string>())
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient"); // Apply CORS policy
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapControllers(); // Map controller routes
app.Run();

