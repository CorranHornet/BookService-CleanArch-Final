using BookService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Application.Genres.Queries
{
    public record GetGenreByIdQuery(int Id) : IRequest<GenreResponseDTO?>;
}
