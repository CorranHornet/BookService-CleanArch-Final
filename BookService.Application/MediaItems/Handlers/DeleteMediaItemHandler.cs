using BookService.Application.MediaItems.Commands;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class DeleteMediaItemHandler
        : IRequestHandler<DeleteMediaItemCommand, bool>
    {
        private readonly IMediaItemRepository _repo;

        public DeleteMediaItemHandler(IMediaItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(
            DeleteMediaItemCommand request,
            CancellationToken cancellationToken)
        {
            var item = await _repo.GetById(request.Id);

            if (item == null)
                return false;

            if (await _repo.HasMediaUnits(request.Id))
                return false;

            await _repo.Delete(item);
            await _repo.Save();

            return true;
        }
    }
}