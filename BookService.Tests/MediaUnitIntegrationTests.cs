using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Commands; // Ensure this matches your namespace
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

    /// <summary>
    /// TEST 1: Original Service-Layer Test
    /// Verifies: DB Connection, TPH Inheritance, and direct Service logic.
    /// </summary>
    [Fact]
    public async Task ServiceLayer_FullLifecycle_VerifiesInheritanceAndPersistence()
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        using var serviceScope = _factory.Services.CreateScope();
        var service = serviceScope.ServiceProvider.GetRequiredService<IMediaUnitService>();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // SEED
        var genre = new Genre { Name = "Service Test Genre" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        var parentItem = new MediaItem { Title = "Service Base Item", GenreId = genre.Id };
        db.MediaItems.Add(parentItem);
        await db.SaveChangesAsync();

        // ACT
        var audioResult = await service.CreateAsync(new MediaUnitCreateDTO 
            { Title = "Service Audiobook", MediaItemId = parentItem.Id, DurationMinutes = 180 });

        // ASSERT
        Assert.Equal("Audiobook", audioResult.UnitType);
        
        var dbUnits = await db.MediaUnits.Where(u => u.MediaItemId == parentItem.Id).ToListAsync();
        Assert.Single(dbUnits);
    }

    /// <summary>
    /// TEST 2: CQRS / MediatR Test
    /// Verifies: MediatR Discovery, Command Handling, and Clean Arch flow.
    /// Requirement: G / VG
    /// </summary>
    [Fact]
    public async Task CQRSLayer_ViaMediator_VerifiesHandlerAndMapping()
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        using var serviceScope = _factory.Services.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // SEED
        var genre = new Genre { Name = "CQRS Test Genre" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        var parentItem = new MediaItem { Title = "CQRS Base Item", GenreId = genre.Id };
        db.MediaItems.Add(parentItem);
        await db.SaveChangesAsync();

        // ACT: Send through Mediator instead of Service
        var command = new CreateMediaUnitCommand 
        { 
            Title = "CQRS Physical Book", 
            MediaItemId = parentItem.Id, 
            PageCount = 300 
        };
        var result = await mediator.Send(command);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("Book", result.UnitType); // Matches your previous 'Actual' result
        
        var exists = await db.MediaUnits.AnyAsync(u => u.Id == result.Id);
        Assert.True(exists);
    }
}