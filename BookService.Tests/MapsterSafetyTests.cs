using Xunit;
using Mapster;
using BookService.Domain.Entities;
using BookService.Application.DTOs;
using BookService.Application.Common.Mapping;

namespace BookService.Tests;

public class MapsterSafetyTests
{
    public MapsterSafetyTests()
    {
        MapsterConfig.Register();
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public void Mapster_Should_Not_Throw_And_Handle_Inheritance()
    {
        var book = new PhysicalBookUnit
        {
            Title = "Clean Code",
            PageCount = 200
        };

        var audio = new AudiobookUnit
        {
            Title = "DDD Audio",
            DurationMinutes = 90
        };

        var dto1 = book.Adapt<MediaUnitResponseDTO>();
        var dto2 = audio.Adapt<MediaUnitResponseDTO>();

        Assert.Equal(200, dto1.PageCount);
        Assert.Equal(90, dto2.DurationMinutes);
    }
}