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
// REPOSITORIES / SERVICES (ALL VITAL PARTS INCLUDED)
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

// ======================================================
// DEVELOPMENT DIAGNOSTIC TEST HARNESS (RESTORED & FULL)
// ======================================================
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("\n======================================");
    Console.WriteLine(" EF CORE / CLEAN ARCH DIAGNOSTIC MODE");
    Console.WriteLine("======================================\n");

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var repo = services.GetRequiredService<IMediaUnitRepository>();
        var service = services.GetRequiredService<IMediaUnitService>();

        // [TEST 0] Schema Inspection
        var entityType = db.Model.FindEntityType(typeof(MediaUnit));
        if (entityType != null)
        {
            Console.WriteLine("[TEST 0] Schema Check: Properties found in MediaUnit:");
            foreach (var prop in entityType.GetProperties())
                Console.WriteLine($" - {prop.Name}");
        }

        // [TEST 1] Direct EF Save - Physical Book
        try
        {
            var book = new PhysicalBookUnit
            {
                Title = "Diagnostic Physical Book",
                PageCount = 350,
                MediaItemId = 1 // Note: This will fail if MediaItem 1 doesn't exist yet!
            };
            db.MediaUnits.Add(book);
            await db.SaveChangesAsync();
            Console.WriteLine("✔ Test 1: PhysicalBookUnit saved via DbContext.");
        }
        catch (Exception ex) { Console.WriteLine($"❌ Test 1 Note: Could not save Book (likely FK constraint): {ex.InnerException?.Message ?? ex.Message}"); }

        // [TEST 2] Repository Save - Audiobook
        try
        {
            var audio = new AudiobookUnit
            {
                Title = "Diagnostic Audiobook",
                DurationMinutes = 420,
                MediaItemId = 1
            };
            await repo.Add(audio);
            await repo.Save();
            Console.WriteLine("✔ Test 2: AudiobookUnit saved via Repository.");
        }
        catch (Exception ex) { Console.WriteLine($"❌ Test 2 Note: Could not save Audiobook: {ex.InnerException?.Message ?? ex.Message}"); }

        // [TEST 3] Service & Mapping - DTO Logic
        try
        {
            var dto = new MediaUnitCreateDTO
            {
                Title = "Service Layer Test",
                DurationMinutes = 120, // This should trigger logic to make it an Audiobook
                MediaItemId = 1
            };
            var result = await service.CreateAsync(dto);
            Console.WriteLine($"✔ Test 3: Service logic created a {result.UnitType} (ID: {result.Id})");
        }
        catch (Exception ex) { Console.WriteLine($"❌ Test 3 Note: Service test failed: {ex.Message}"); }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Critical failure in diagnostic harness: {ex.Message}");
    }
}

app.Run();