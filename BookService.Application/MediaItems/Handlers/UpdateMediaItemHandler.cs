using BookService.Application.MediaItems.Commands;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class UpdateMediaItemHandler
        : IRequestHandler<UpdateMediaItemCommand, bool>
    {
        private readonly IMediaItemRepository _repo;

        public UpdateMediaItemHandler(IMediaItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(
            UpdateMediaItemCommand request,
            CancellationToken cancellationToken)
        {
            var item = await _repo.GetById(request.Id);

            if (item == null)
                return false;

            if (!string.IsNullOrWhiteSpace(request.Title))
                item.Title = request.Title;

            if (request.GenreId.HasValue)
            {
                var exists = await _repo.GenreExists(request.GenreId.Value);
                if (!exists) return false;

                item.GenreId = request.GenreId.Value;
            }

            await _repo.Save();
            return true;
        }
    }
}