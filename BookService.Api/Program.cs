using BookService.Application.Common.Mapping;
using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using BookService.Infrastructure.Repositories;
using BookService.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// SWAGGER & API EXPLORER
// ======================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================================
// MEDIATR
// ======================================================
// Scanning the assembly where MediaUnitDTO lives to find all handlers
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(MediaUnitDTO).Assembly));

// ======================================================
// CONTROLLERS
// ======================================================
builder.Services.AddControllers();

// ======================================================
// DB CONTEXT
// ======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ======================================================
// REPOSITORIES / SERVICES
// ======================================================
builder.Services.AddScoped<IMediaItemRepository, MediaItemRepository>();
builder.Services.AddScoped<IMediaUnitRepository, MediaUnitRepository>();
builder.Services.AddScoped<IMediaUnitService, MediaUnitService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthService, AuthService>(); 

// ======================================================
// MAPSTER CONFIGURATION
// ======================================================
MapsterConfig.Register();
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();

// ======================================================
// PIPELINE & SWAGGER UI
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

// This partial class makes the Program class visible to your xUnit project.
// Without this, the Integration Tests won't be able to "boot" the API.
public partial class Program { }