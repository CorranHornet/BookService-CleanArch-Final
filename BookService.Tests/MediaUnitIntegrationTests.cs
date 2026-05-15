using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Xunit;

namespace BookService.Tests;

public class MediaUnitIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MediaUnitIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task MediaUnit_FullLifecycle_VerifiesInheritanceAndPersistence()
    {
        // Start transaction - everything inside here is "temporary"
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        using var serviceScope = _factory.Services.CreateScope();
        var service = serviceScope.ServiceProvider.GetRequiredService<IMediaUnitService>();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // 1. SEED GENRE
        var genre = new Genre { Name = "Test Genre" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        // 2. SEED MEDIAITEM
        var parentItem = new MediaItem 
        { 
            Title = "Test Base Item", 
            Description = "Testing Foundation",
            GenreId = genre.Id 
        };
        db.MediaItems.Add(parentItem);
        await db.SaveChangesAsync();

        // 3. ACT: Create Audiobook via Service
        var audioDto = new MediaUnitCreateDTO 
        { 
            Title = "Complete Test Audiobook", 
            MediaItemId = parentItem.Id, 
            DurationMinutes = 180 
        };
        var audioResult = await service.CreateAsync(audioDto);

        // 4. ACT: Create Physical Book via Service
        var bookDto = new MediaUnitCreateDTO 
        { 
            Title = "Complete Test Physical Book", 
            MediaItemId = parentItem.Id, 
            PageCount = 250 
        };
        var bookResult = await service.CreateAsync(bookDto);

        // 5. ASSERT: Check Service Logic
        Assert.Equal("Audiobook", audioResult.UnitType);
        Assert.Equal("Book", bookResult.UnitType);

        // 6. ASSERT: Check Database TPH Mapping
        var dbUnits = await db.MediaUnits
            .Where(u => u.MediaItemId == parentItem.Id)
            .ToListAsync();
            
        Assert.Equal(2, dbUnits.Count);
        Assert.Contains(dbUnits, u => u is AudiobookUnit);
        Assert.Contains(dbUnits, u => u is PhysicalBookUnit);

        // Transaction closes here -> SQL Server rolls back everything!
    }
} // Ends Class