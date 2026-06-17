using BookService.Application.Interfaces;
using BookService.Application.MediaItems.Commands;
using MapsterMapper;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class DeleteMediaItemHandler
        : IRequestHandler<DeleteMediaItemCommand, bool>
    {
        private readonly IMediaItemRepository _repo;
        private readonly IMapper _mapper;

        public DeleteMediaItemHandler(IMediaItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}