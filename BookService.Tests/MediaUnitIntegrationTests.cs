using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Commands;
using BookService.Application.Common.Mapping; // Matches your MapsterConfig namespace
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Mapster;
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

        // ACT: Send through Mediator
        var command = new CreateMediaUnitCommand
        {
            Title = "CQRS Physical Book",
            MediaItemId = parentItem.Id,
            PageCount = 300
        };
        var result = await mediator.Send(command);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("Book", result.UnitType);
        Assert.Equal(300, result.PageCount);

        var exists = await db.MediaUnits.AnyAsync(u => u.Id == result.Id);
        Assert.True(exists);
    }

    /// <summary>
    /// TEST 3: Mapster Configuration Validation
    /// Verifies: That the static MapsterConfig is registered and valid.
    /// </summary>
    [Fact]
    public void MappingConfiguration_ShouldBeValidAndCompilable()
    {
        // Act: Initialize your static config
        MapsterConfig.Register();

        // Assert: Ensure Global Settings pass the compiler check
        // This validates that destination properties can be mapped from source properties
        TypeAdapterConfig.GlobalSettings.Compile();
        //TypeAdapterConfig.GlobalSettings.Validate();
    }

    /// <summary>
    /// TEST 4: Mapping Logic Test
    /// Verifies: That specific inheritance and property logic works.
    /// </summary>
    [Fact]
    public void Mapster_ShouldCorrectlyMapMediaUnitInheritance()
    {
        // Arrange
        MapsterConfig.Register();
        var audiobook = new AudiobookUnit { Title = "Audio Test", DurationMinutes = 45 };
        var book = new PhysicalBookUnit { Title = "Book Test", PageCount = 150 };

        // Act
        var audioDto = audiobook.Adapt<MediaUnitResponseDTO>();
        var bookDto = book.Adapt<MediaUnitResponseDTO>();

        // Assert
        Assert.Equal("Audiobook", audioDto.UnitType);
        Assert.Equal(45, audioDto.DurationMinutes);
        Assert.Equal("Book", bookDto.UnitType);
        Assert.Equal(150, bookDto.PageCount);
    }
}