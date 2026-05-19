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

        // Mapster handles basic mapping (NO manual property assignment)
        var entity = request.Adapt<MediaUnit>();

        // Only branching logic allowed = domain decision, not mapping
        if (request.DurationMinutes.HasValue && request.DurationMinutes > 0)
            entity = new AudiobookUnit { DurationMinutes = request.DurationMinutes.Value };
        else entity = new PhysicalBookUnit { PageCount = request.PageCount ?? 0 };

        //Mapster again for shared Properties
        request.Adapt(entity);
        await _repo.Save();

        return entity.Adapt<MediaUnitResponseDTO>();
    }
}


        