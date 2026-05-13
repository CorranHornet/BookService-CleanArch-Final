using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using BookService.Infrastructure.Repositories;
using BookService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// SERVICES (DI SETUP)
// -----------------------------
builder.Services.AddControllers();

// DbContext (your Infrastructure layer)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories + Services
builder.Services.AddScoped<IMediaUnitRepository, MediaUnitRepository>();
builder.Services.AddScoped<IMediaUnitService, MediaUnitService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();


// ======================================================
// 🧪 DIAGNOSTIC TEST HARNESS
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
    // TEST 0 — EF MODEL INSPECTION (VERY IMPORTANT)
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
    // TEST 1 — DIRECT DB CONTEXT INSERT
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
            // IsAvailable intentionally missing (if exists in DB)
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
    // TEST 2 — REPOSITORY LAYER
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
    // TEST 3 — SERVICE LAYER (REAL BUSINESS FLOW)
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


    // --------------------------------------------------
    // TEST 4 — RAW DATABASE READBACK
    // --------------------------------------------------
    try
    {
        Console.WriteLine("\n[TEST 4] Read-back validation");

        var last = await db.MediaUnits
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (last != null)
        {
            Console.WriteLine("✔ Read SUCCESS");
            Console.WriteLine($"Title: {last.Title}");

            var isAvailableProp = last.GetType().GetProperty("IsAvailable");

            if (isAvailableProp != null)
            {
                Console.WriteLine($"IsAvailable: {isAvailableProp.GetValue(last)}");
            }
            else
            {
                Console.WriteLine("⚠ IsAvailable DOES NOT EXIST on entity model");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ READ FAILED");
        Console.WriteLine(ex.Message);
    }


    // --------------------------------------------------
    // TEST 5 — FORCED FAILURE (REPRODUCE YOUR BUG)
    // --------------------------------------------------
    try
    {
        Console.WriteLine("\n[TEST 5] Forced NULL insert (BUG REPRODUCE)");

        var badEntity = new MediaUnit
        {
            Title = "FORCED FAILURE TEST",
            Number = 99,
            DurationMinutes = 10,
            MediaItemId = 1
            // IsAvailable missing again intentionally
        };

        db.MediaUnits.Add(badEntity);
        await db.SaveChangesAsync();

        Console.WriteLine("✔ Unexpected success (check schema!)");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ EXPECTED FAILURE OCCURRED");
        Console.WriteLine("ROOT CAUSE:");
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
    }


    Console.WriteLine("\n======================================");
    Console.WriteLine(" DIAGNOSTIC COMPLETE");
    Console.WriteLine("======================================\n");
}

app.Run();