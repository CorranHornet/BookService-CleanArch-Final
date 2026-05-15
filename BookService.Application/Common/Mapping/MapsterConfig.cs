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

            // Basic Mappings (Name matching works automatically here)
            config.NewConfig<User, UserDTO>();
            config.NewConfig<User, LoginResponseDTO>()
                .Ignore(dest => dest.Token);
            
            
            config.NewConfig<Genre, GenreResponseDTO>();
            config.NewConfig<Loan, LoanResponseDTO>();
            config.NewConfig<User, UserDTO>();
            config.NewConfig<MediaUnit, MediaUnitDTO>();

            // Advanced Mapping for MediaUnit -> MediaUnitResponseDTO
            // This handles the inheritance logic for the "UnitType" string
            // and casts the entity to the correct type to get PageCount/Duration.
            config.NewConfig<MediaUnit, MediaUnitResponseDTO>()
                .Map(dest => dest.UnitType, src => src is PhysicalBookUnit ? "Book" : "Audiobook")
                .Map(dest => dest.PageCount, src => src is PhysicalBookUnit
                    ? ((PhysicalBookUnit)src).PageCount
                    : (int?)null)
                .Map(dest => dest.DurationMinutes, src => src is AudiobookUnit
                    ? ((AudiobookUnit)src).DurationMinutes
                    : (int?)null);
        }
    }
}