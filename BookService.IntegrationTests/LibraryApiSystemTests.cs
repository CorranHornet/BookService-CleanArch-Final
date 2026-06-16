using BookService.Application;           // DI Application-layer
using BookService.Infrastructure;        // DI Infrastructure-layer
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// LAGER-SPECIFIKA REGISTRERINGAR
// ======================================================
builder.Services.AddApplication();           // Handlers, Validators, Behaviors
builder.Services.AddInfrastructure(builder.Configuration); // DbContext, Repositories, Test-logik

// ======================================================
// API-SPECIFIK KONFIGURATION
// ======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================================
// AUTHENTICATION & JWT BEARER
// ======================================================
var tokenKey = builder.Configuration["AppSettings:Token"]
               ?? "TESTING_SECRET_KEY_THAT_IS_LONG_ENOUGH_TO_BE_SECURE_1234567890";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ======================================================
// MIDDLEWARE PIPELINE
// ======================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }