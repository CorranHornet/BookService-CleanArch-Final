using Mapster;
using BookService.Domain.Entities;
using BookService.Application.DTOs;
using BookService.Application.Common.Mapping;
using Xunit;

namespace BookService.Tests;

public class MapsterStrictTests
{
    public MapsterStrictTests()
    {
        MapsterConfig.Register();

        // Validates configuration only (safe, supported)
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public void Mapster_Should_Map_Domain_To_DTO()
    {
        var book = new PhysicalBookUnit
        {
            Title = "Test Book",
            PageCount = 123
        };

        var dto = book.Adapt<MediaUnitDTO>();

        Assert.Equal("Test Book", dto.Title);
        Assert.Equal(123, dto.PageCount);
    }

    [Fact]
    public void Mapster_Should_Not_Fail_On_Inheritance()
    {
        MediaUnit unit = new AudiobookUnit
        {
            Title = "Audio Test",
            DurationMinutes = 45
        };

        var dto = unit.Adapt<MediaUnitDTO>();

        Assert.Equal("Audio Test", dto.Title);
    }
}