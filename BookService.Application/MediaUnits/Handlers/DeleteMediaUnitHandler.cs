using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.MediaUnits.Commands;

namespace BookService.Application.MediaUnits.Handlers;

public class DeleteMediaUnitHandler : IRequestHandler<DeleteMediaUnitCommand, bool>
{
    private readonly IMediaUnitRepository _repo;
    public DeleteMediaUnitHandler(IMediaUnitRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteMediaUnitCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetById(request.Id);
        if (entity == null) return false;

        if (await _repo.HasActiveLoan(request.Id)) return false;

        await _repo.Delete(entity);
        await _repo.Save();
        return true;
    }
}