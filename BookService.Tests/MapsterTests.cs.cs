using Mapster;
using BookService.Application.Common.Mapping;
using BookService.Application.DTOs;
using BookService.Domain.Entities;

namespace BookService.Tests
{
    public class MapsterTests
    {
        // ------------------------------------------------------------
        // 1. CONFIG MUST COMPILE (this catches 80% of errors early)
        // ------------------------------------------------------------
        [Fact]
        public void MapsterConfig_ShouldCompileWithoutErrors()
        {
            MapsterConfig.Register();

            var config = TypeAdapterConfig.GlobalSettings;

            config.Compile();

            Assert.True(true); // if we reach here, mapping compiled
        }

        // ------------------------------------------------------------
        // 2. DOMAIN → DTO mapping MUST WORK
        // ------------------------------------------------------------
        [Fact]
        public void Mapster_ShouldMap_PhysicalBookUnit_ToDTO()
        {
            MapsterConfig.Register();

            var entity = new PhysicalBookUnit
            {
                Id = 1,
                Title = "Clean Code",
                PageCount = 450,
                MediaItemId = 10
            };

            var dto = entity.Adapt<MediaUnitDTO>();

            Assert.NotNull(dto);
            Assert.Equal("Clean Code", dto.Title);
            Assert.Equal(450, dto.PageCount);
        }

        // ------------------------------------------------------------
        // 3. INHERITANCE MUST BE HANDLED CORRECTLY
        // ------------------------------------------------------------
        [Fact]
        public void Mapster_ShouldHandle_Inheritance_ForMediaUnit()
        {
            MapsterConfig.Register();

            MediaUnit book = new PhysicalBookUnit
            {
                Id = 2,
                Title = "Domain Driven Design",
                PageCount = 500,
                MediaItemId = 20
            };

            var dto = book.Adapt<MediaUnitDTO>();

            Assert.NotNull(dto);
            Assert.Equal("Domain Driven Design", dto.Title);
            Assert.Equal(500, dto.PageCount);
        }

        // ------------------------------------------------------------
        // 4. AUDIobook mapping (second derived type)
        // ------------------------------------------------------------
        [Fact]
        public void Mapster_ShouldMap_AudiobookUnit_ToDTO()
        {
            MapsterConfig.Register();

            var entity = new AudiobookUnit
            {
                Id = 3,
                Title = "Clean Architecture Audio",
                DurationMinutes = 120,
                MediaItemId = 30
            };

            var dto = entity.Adapt<MediaUnitDTO>();

            Assert.NotNull(dto);
            Assert.Equal("Clean Architecture Audio", dto.Title);
            Assert.Equal(120, dto.DurationMinutes);
        }

        // ------------------------------------------------------------
        // 5. GUARANTEE no null mapping failures at runtime
        // ------------------------------------------------------------
        [Fact]
        public void Mapster_ShouldNotThrow_WhenMappingValidEntities()
        {
            MapsterConfig.Register();

            var book = new PhysicalBookUnit
            {
                Title = "Test",
                PageCount = 1,
                MediaItemId = 1
            };

            var ex = Record.Exception(() =>
            {
                var dto = book.Adapt<MediaUnitDTO>();
            });

            Assert.Null(ex);
        }
    }
}