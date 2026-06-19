using BookService.Application.Common.Mapping;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Commands;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;
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
    public async Task CQRS_MediaUnit_Create_ShouldWorkWithoutServiceLayer()
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        using var serviceScope = _factory.Services.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var genre = new Genre { Name = "Test Genre" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        var parentItem = new MediaItem { Title = "Base Item", GenreId = genre.Id };
        db.MediaItems.Add(parentItem);
        await db.SaveChangesAsync();

        var command = new CreateMediaUnitCommand
        {
            Title = "Test Book",
            MediaItemId = parentItem.Id,
            PageCount = 200
        };

        var result = await mediator.Send(command);

        Assert.NotNull(result);
        Assert.Equal("Book", result.UnitType);
    }

    [Fact]
    public async Task CQRSLayer_ViaMediator_VerifiesHandlerAndMapping()
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        using var serviceScope = _factory.Services.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var db = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var genre = new Genre { Name = "CQRS Test Genre" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        var parentItem = new MediaItem { Title = "CQRS Base Item", GenreId = genre.Id };
        db.MediaItems.Add(parentItem);
        await db.SaveChangesAsync();

        var command = new CreateMediaUnitCommand
        {
            Title = "CQRS Physical Book",
            MediaItemId = parentItem.Id,
            PageCount = 300
        };

        var result = await mediator.Send(command);

        Assert.NotNull(result);
        Assert.Equal("Book", result.UnitType);
        Assert.Equal(300, result.PageCount);

        var exists = await db.MediaUnits.AnyAsync(u => u.Id == result.Id);
        Assert.True(exists);
    }

    [Fact]
    public void MappingConfiguration_ShouldBeValidAndCompilable()
    {
        MapsterConfig.Register();
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public void Mapster_ShouldCorrectlyMapMediaUnitInheritance()
    {
        MapsterConfig.Register();

        var audiobook = new AudiobookUnit { Title = "Audio Test", DurationMinutes = 45 };
        var book = new PhysicalBookUnit { Title = "Book Test", PageCount = 150 };

        var audioDto = audiobook.Adapt<MediaUnitDTO>();
        var bookDto = book.Adapt<MediaUnitDTO>();

        Assert.Equal("Audiobook", audioDto.UnitType);
        Assert.Equal(45, audioDto.DurationMinutes);

        Assert.Equal("Book", bookDto.UnitType);
        Assert.Equal(150, bookDto.PageCount);
    }
}