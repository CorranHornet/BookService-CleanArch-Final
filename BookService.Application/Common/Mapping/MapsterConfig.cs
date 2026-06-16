using Mapster;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Application.MediaUnits.Commands;

namespace BookService.Application.Common.Mapping
{
    public static class MapsterConfig
    {
        public static void Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Basic mappings
            config.NewConfig<MediaItem, MediaItemResponseDTO>()
                .Map(dest => dest.Genre, src => src.Genre != null ? src.Genre.Name : "")
                .Map(dest => dest.GenreId, src => src.GenreId);

            config.NewConfig<User, UserDTO>();

            config.NewConfig<User, LoginResponseDTO>()
                .Ignore(dest => dest.Token);

            config.NewConfig<Genre, GenreResponseDTO>();

            config.NewConfig<Loan, LoanResponseDTO>();

            config.NewConfig<MediaUnit, MediaUnitDTO>();

            config.NewConfig<CreateMediaUnitCommand, PhysicalBookUnit>()
                .Map(dest => dest.PageCount, src => src.PageCount);

            config.NewConfig<CreateMediaUnitCommand, AudiobookUnit>()
               .Map(dest => dest.DurationMinutes, src =>src.DurationMinutes);

            // =========================================================
            // MediaUnit -> MediaUnitResponseDTO
            // =========================================================

            config.NewConfig<MediaUnit, MediaUnitResponseDTO>()
                .Map(dest => dest.UnitType,
                    src => src is PhysicalBookUnit ? "Book" : "Audiobook")

                .Map(dest => dest.PageCount,
                    src => src is PhysicalBookUnit
                        ? ((PhysicalBookUnit)src).PageCount
                        : 0)

                .Map(dest => dest.DurationMinutes,
                    src => src is AudiobookUnit
                        ? ((AudiobookUnit)src).DurationMinutes
                        : 0);
        }
    }
}