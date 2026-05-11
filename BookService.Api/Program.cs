using BookService.Application.Interfaces;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BookService.Infrastructure.Services;
using BookService.Application.Interfaces;
//using BookService.Infrastructure.Services;
using BookService.Infrastructure.Repositories;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// 1. OPENAPI / SCALAR
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Klistra in din JWT-token här!"
        };
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", scheme);
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();

// 2. DATABAS WIRING (MATCHING YOUR NAMES)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Bridge between Interface and Context
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// 3. SERVICES (Keep your existing ones)
// builder.Services.AddScoped<IAuthService, AuthService>... 
builder.Services.AddScoped<IMediaItemService, MediaItemService>();
builder.Services.AddScoped<IMediaUnitService, MediaUnitService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMediaItemService, MediaItemService>();


builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();

// 4. JWT & AUTH (Keep your existing logic)
var key = Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy => policy.WithOrigins("http://localhost:5500").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();