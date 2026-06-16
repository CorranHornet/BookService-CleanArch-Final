using BookService.Api;           
using BookService.Application;   
using BookService.Infrastructure;  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookService.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// LAYERED DEPENDENCY INJECTION
// ======================================================
builder.Services.AddApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ======================================================
// AUTHENTICATION
// ======================================================
var tokenKey = builder.Configuration["AppSettings:Token"]
    ?? "TESTING_SECRET_KEY_THAT_IS_LONG_ENOUGH_TO_BE_SECURE_1234567890";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

var app = builder.Build();


app.UseMiddleware<RequestTracingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// ======================================================
// MIDDLEWARE PIPELINE
// ======================================================
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