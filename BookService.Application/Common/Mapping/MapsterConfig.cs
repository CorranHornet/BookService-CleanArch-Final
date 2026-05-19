using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using BookService.Application.DTOs;
using BookService.Domain.Entities;

namespace BookService.Application.Common.Mapping
{
    public static class MapsterConfig
    {
        public static void Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Basic mappings
            config.NewConfig<User, UserDTO>();

            config.NewConfig<User, LoginResponseDTO>()
                .Ignore(dest => dest.Token);

            config.NewConfig<Genre, GenreResponseDTO>();

            config.NewConfig<Loan, LoanResponseDTO>();

            config.NewConfig<MediaUnit, MediaUnitDTO>();


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