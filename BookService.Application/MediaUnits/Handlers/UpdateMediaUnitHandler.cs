using MediatR;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Application.MediaUnits.Commands;
using MapsterMapper;

namespace BookService.Application.MediaUnits.Handlers;

public class UpdateMediaUnitHandler : IRequestHandler<UpdateMediaUnitCommand, bool>
{
    private readonly IMediaUnitRepository _repo;
    private readonly IMapper _mapper;
    public UpdateMediaUnitHandler(IMediaUnitRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateMediaUnitCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetById(request.Id);
        if (entity == null) return false;

        if (!string.IsNullOrWhiteSpace(request.Title)) entity.Title = request.Title;
        entity.Number = request.Number ?? entity.Number;

        if (entity is PhysicalBookUnit book && request.PageCount.HasValue) 
            book.PageCount = request.PageCount.Value;
        else if (entity is AudiobookUnit audio && request.DurationMinutes.HasValue) 
            audio.DurationMinutes = request.DurationMinutes.Value;

        await _repo.SaveChangesAsync();
        return true;
    }
}