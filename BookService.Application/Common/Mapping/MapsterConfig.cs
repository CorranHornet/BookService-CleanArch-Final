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

            TypeAdapterConfig<Loan, LoanResponseDTO>.NewConfig();
            TypeAdapterConfig<User, UserDTO>.NewConfig();
            TypeAdapterConfig<MediaUnit, MediaUnitDTO>.NewConfig();
        }
    }
}
