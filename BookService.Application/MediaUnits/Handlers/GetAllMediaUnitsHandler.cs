using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using BookService.Domain.Entities;

namespace BookService.Application.MediaUnits.Handlers;

public class GetAllMediaUnitsHandler : IRequestHandler<GetAllMediaUnitsQuery, IEnumerable<MediaUnitResponseDTO>>
{
    private readonly IMediaUnitRepository _repo;
    public GetAllMediaUnitsHandler(IMediaUnitRepository repo) => _repo = repo;

    public async Task<IEnumerable<MediaUnitResponseDTO>> Handle(GetAllMediaUnitsQuery request, CancellationToken ct)
    {
        var units = await _repo.GetAll();
        return units.Select(mu => new MediaUnitResponseDTO {
            Id = mu.Id,
            Title = mu.Title,
            Number = mu.Number,
            MediaItemId = mu.MediaItemId,
            UnitType = mu is PhysicalBookUnit ? "Book" : "Audiobook"
        });
    }
}