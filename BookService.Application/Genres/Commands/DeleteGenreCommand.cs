using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Application.Genres.Commands
{
    public record DeleteGenreCommand(int Id) : IRequest<bool>;
}
