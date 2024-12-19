// this is the API Program.cs
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
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
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
builder.Services.AddHttpClient();

// Add database context

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=database.db"), ServiceLifetime.Scoped);

builder.Services.AddScoped<IAuthServiceAPI, AuthServiceAPI>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//     {
//         Title = "AquAnalyzer API",
//         Version = "v1",
//         Description = "API for AquAnalyzer application",
//     });
//     options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//     {
//         Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
//         Name = "Authorization",
//         In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//         Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//         Scheme = "Bearer"
//     });
//     options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//     {
//         {
//             new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//             {
//                 Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                 {
//                     Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             Array.Empty<string>()
//         }
//     });
// });

// Add JWT-based authentication directly
// var jwtKey = builder.Configuration["Jwt:Key"];
// var jwtIssuer = builder.Configuration["Jwt:Issuer"];
// var jwtAudience = builder.Configuration["Jwt:Audience"];

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidAudience = jwtAudience,
//         ValidIssuer = jwtIssuer,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//         ClockSkew = TimeSpan.Zero,
//     };
// });

// // Add authorization policies
// AuthorizationPolicies.AddPolicies(builder.Services);


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
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors("AllowBlazorClient"); // Apply CORS policy
// app.UseAuthentication(); 
// app.UseAuthorization();  // Enable authorization middleware
app.MapControllers(); // Map controller routes
app.Run();

