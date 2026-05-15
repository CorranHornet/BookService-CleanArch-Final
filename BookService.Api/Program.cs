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
// CONTROLLERS
// ======================================================
builder.Services.AddControllers();

// ======================================================
// DB CONTEXT (Infrastructure)
// ======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ======================================================
// REPOSITORIES / SERVICES
// ======================================================
builder.Services.AddScoped<IMediaUnitRepository, MediaUnitRepository>();
builder.Services.AddScoped<IMediaUnitService, MediaUnitService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// ======================================================
// MAPSTER CONFIGURATION (FIX FOR YOUR ERROR)
// ======================================================

// register your mapping rules
MapsterConfig.Register();

// register Mapster DI dependencies
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// ======================================================
var app = builder.Build();

// ======================================================
// PIPELINE
// ======================================================
app.UseRouting();

app.MapControllers();

// ======================================================
// DEVELOPMENT DIAGNOSTIC TEST HARNESS
// ======================================================
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("\n======================================");
    Console.WriteLine(" EF CORE / CLEAN ARCH DIAGNOSTIC MODE");
    Console.WriteLine("======================================\n");

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var repo = scope.ServiceProvider.GetRequiredService<IMediaUnitRepository>();
    var service = scope.ServiceProvider.GetRequiredService<IMediaUnitService>();

    // --------------------------------------------------
    // TEST 0 — EF MODEL INSPECTION
    // --------------------------------------------------
    Console.WriteLine("[TEST 0] EF Core Model Inspection");

    var entityType = db.Model.FindEntityType(typeof(MediaUnit));

    if (entityType == null)
    {
        Console.WriteLine("❌ MediaUnit not found in EF model!");
    }
    else
    {
        foreach (var prop in entityType.GetProperties())
        {
            Console.WriteLine($"Property: {prop.Name} | Nullable: {prop.IsNullable}");
        }
    }

    // --------------------------------------------------
    // TEST 1 — DB CONTEXT
    // --------------------------------------------------
    try
    {
        Console.WriteLine("\n[TEST 1] Direct DbContext insert");

        var entity = new MediaUnit
        {
            Title = "DbContext Test",
            Number = 1,
            DurationMinutes = 100,
            MediaItemId = 1
        };

        db.MediaUnits.Add(entity);
        await db.SaveChangesAsync();

        Console.WriteLine("✔ DbContext insert SUCCESS");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ DbContext insert FAILED");
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
    }

    // --------------------------------------------------
    // TEST 2 — REPOSITORY
    // --------------------------------------------------
    try
    {
        Console.WriteLine("\n[TEST 2] Repository insert");

        var entity = new MediaUnit
        {
            Title = "Repo Test",
            Number = 2,
            DurationMinutes = 80,
            MediaItemId = 1
        };

        await repo.Add(entity);
        await repo.Save();

        Console.WriteLine("✔ Repository insert SUCCESS");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Repository FAILED");
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
    }

    // --------------------------------------------------
    // TEST 3 — SERVICE
    // --------------------------------------------------
    try
    {
        Console.WriteLine("\n[TEST 3] Service CreateAsync");

        var dto = new MediaUnitCreateDTO
        {
            Title = "Service Test",
            Number = 3,
            DurationMinutes = 60,
            MediaItemId = 1
        };

        var result = await service.CreateAsync(dto);

        Console.WriteLine("✔ Service SUCCESS");
        Console.WriteLine($"ID: {result.Id}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ SERVICE FAILED");
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
    }

    Console.WriteLine("\n======================================");
    Console.WriteLine(" DIAGNOSTIC COMPLETE");
    Console.WriteLine("======================================\n");
}

app.Run();