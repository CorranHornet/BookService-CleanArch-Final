using BookService.Application.Genres.Commands;
using BookService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Application.Genres.Handlers
{
    internal class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, bool>
    {
        private readonly IGenreRepository _repo;

        public DeleteGenreCommandHandler(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken ct)
        {
            var genre = await _repo.GetById(request.Id);
            if (genre == null) return false;

            if (await _repo.HasMediaItems(request.Id))
                return false;

            await _repo.Delete(genre);
            await _repo.Save();

            return true;
        }
    }
}
