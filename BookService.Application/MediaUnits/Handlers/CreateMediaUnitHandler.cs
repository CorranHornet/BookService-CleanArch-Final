using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Application.MediaUnits.Commands;
using Mapster;

namespace BookService.Application.MediaUnits.Handlers;

public class CreateMediaUnitHandler : IRequestHandler<CreateMediaUnitCommand, MediaUnitResponseDTO?>
{
    private readonly IMediaUnitRepository _repo;

    public CreateMediaUnitHandler(IMediaUnitRepository repo)
    {
        _repo = repo;
    }

    // The signature must match the interface exactly:
    // 1. Must be 'public'
    // 2. Return type: Task<MediaUnitResponseDTO?>
    // 3. First param: CreateMediaUnitCommand
    // 4. Second param: CancellationToken
    public async Task<MediaUnitResponseDTO?> Handle(CreateMediaUnitCommand request, CancellationToken ct)
    {
        if (!await _repo.MediaItemExists(request.MediaItemId))
            return null;

        MediaUnit entity = (request.DurationMinutes.HasValue && request.DurationMinutes.Value > 0)
            ? new AudiobookUnit { DurationMinutes = request.DurationMinutes.Value }
            : new PhysicalBookUnit { PageCount = request.PageCount ?? 0 };

        entity.Title = request.Title;
        entity.Number = request.Number;
        entity.MediaItemId = request.MediaItemId;

        await _repo.Add(entity);
        await _repo.Save();

        return entity.Adapt<MediaUnitResponseDTO>();
    }
}