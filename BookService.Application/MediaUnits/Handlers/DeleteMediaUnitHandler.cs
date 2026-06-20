using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.MediaUnits.Commands;
using MapsterMapper;

namespace BookService.Application.MediaUnits.Handlers
{
    public class DeleteMediaUnitHandler : IRequestHandler<DeleteMediaUnitCommand, bool>
    {
        private readonly IMediaUnitRepository _repo;
        private readonly IMapper _mapper;

        // Corrected constructor to include _mapper
        public DeleteMediaUnitHandler(IMediaUnitRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteMediaUnitCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetById(request.Id);

            if (entity == null)
                return false;

            // Business rule check: Cannot delete a unit that is currently on loan
            if (await _repo.HasActiveLoan(request.Id))
                return false;

            await _repo.Delete(entity);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}