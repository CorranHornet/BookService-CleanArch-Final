using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MediatR;
using BookService.Application.MediaUnits.Commands;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;

namespace BookService.Tests;

public class SystemFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _db;

    public SystemFlowTests(WebApplicationFactory<Program> factory)
    {
        var scope = factory.Services.CreateScope();

        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task Full_CQRS_Flow_Should_Work_End_To_End()
    {
        // -----------------------------
        // ARRANGE (seed required data)
        // -----------------------------
        var genre = new Genre { Name = "Test Genre" };
        _db.Genres.Add(genre);
        await _db.SaveChangesAsync();

        var mediaItem = new MediaItem
        {
            Title = "Base Item",
            GenreId = genre.Id
        };

        _db.MediaItems.Add(mediaItem);
        await _db.SaveChangesAsync();

        // -----------------------------
        // ACT (CQRS + Mapster + Handler)
        // -----------------------------
        var command = new CreateMediaUnitCommand
        {
            Title = "Integration Book",
            MediaItemId = mediaItem.Id,
            PageCount = 321
        };

        var result = await _mediator.Send(command);

        // -----------------------------
        // ASSERT (everything together)
        // -----------------------------
        Assert.NotNull(result);
        Assert.Equal("Book", result.UnitType);
        Assert.Equal(321, result.PageCount);
        Assert.Equal("Integration Book", result.Title);

        var dbEntity = await _db.MediaUnits.FirstOrDefaultAsync(x => x.Id == result.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal(mediaItem.Id, dbEntity!.MediaItemId);
    }
}