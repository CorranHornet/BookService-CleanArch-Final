using BookService.Application.DTOs;
using BookService.Application.MediaItems.Commands;
using BookService.Application.MediaUnits.Commands;
using BookService.Domain.Entities;
using Mapster;

namespace BookService.Application.Common.Mapping
{
    public static class MapsterConfig
    {
        public static void Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // =========================================================
            // MEDIA ITEM
            // =========================================================
            config.NewConfig<MediaItem, MediaItemResponseDTO>()
                .Map(dest => dest.Genre,
                    src => src.Genre != null ? src.Genre.Name : string.Empty);

            config.NewConfig<CreateMediaItemCommand, MediaItem>();

            // =========================================================
            // USER
            // =========================================================
            config.NewConfig<User, UserDTO>();

            config.NewConfig<User, LoginResponseDTO>()
                .Ignore(dest => dest.Token);

            // =========================================================
            // GENRE / LOAN
            // =========================================================
            config.NewConfig<Genre, GenreResponseDTO>();
            config.NewConfig<Loan, LoanResponseDTO>();

            // =========================================================
            // MEDIA UNIT (READ DTO)
            // =========================================================
            config.NewConfig<MediaUnit, MediaUnitDTO>()
                .Map(dest => dest.UnitType,
                    src => src is PhysicalBookUnit ? "Book" : "Audiobook")

                .Map(dest => dest.PageCount,
                    src => src is PhysicalBookUnit
                        ? ((PhysicalBookUnit)src).PageCount
                        : (int?)null)

                .Map(dest => dest.DurationMinutes,
                    src => src is AudiobookUnit
                        ? ((AudiobookUnit)src).DurationMinutes
                        : (int?)null);

            // =========================================================
            // CREATE MEDIA UNIT (WRITE SIDE)
            // =========================================================

            config.NewConfig<CreateMediaUnitCommand, PhysicalBookUnit>()
                .Map(dest => dest.PageCount,
                    src => src.PageCount.HasValue ? src.PageCount.Value : 0);

            config.NewConfig<CreateMediaUnitCommand, AudiobookUnit>()
                .Map(dest => dest.DurationMinutes,
                    src => src.DurationMinutes.HasValue ? src.DurationMinutes.Value : 0);

            // =========================================================
            // OPTIONAL: explicit reverse mapping safety (helps debugging)
            // =========================================================
            config.NewConfig<PhysicalBookUnit, MediaUnitDTO>();
            config.NewConfig<AudiobookUnit, MediaUnitDTO>();
        }
    }
}