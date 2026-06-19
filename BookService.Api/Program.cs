using BookService.Api;
using BookService.Api.Middleware;
using BookService.Application;
using BookService.Application.Common.Settings;
using BookService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Required for Swagger Security Schemes

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// LAYERED DEPENDENCY INJECTION
// ======================================================
builder.Services.AddApi();
builder.Services.AddApplication();

// Bind configuration to JwtSettings class and register it for DI
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddInfrastructure(builder.Configuration);

// Add Swagger with JWT Authorization support
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Standard för JWT
        BearerFormat = "JWT",
        Description = "Klistra in din JWT-token nedan. Skriv INTE 'Bearer ' själv."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

// ======================================================
// AUTHENTICATION
// ======================================================
var serviceProvider = builder.Services.BuildServiceProvider();
var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

if (string.IsNullOrWhiteSpace(jwtSettings.Token) || Convert.FromBase64String(jwtSettings.Token.Trim()).Length < 64)
{
    throw new InvalidOperationException("JWT Secret Key is missing or too short! It must be at least 64 bytes (512 bits) when Base64 encoded.");
}

var keyBytes = Convert.FromBase64String(jwtSettings.Token.Trim());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"DEBUG: Auth Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// ======================================================
// MIDDLEWARE PIPELINE
// ======================================================
// Viktigt: Authentication/Authorization måste ske innan dina egna middlewares
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestTracingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program { }