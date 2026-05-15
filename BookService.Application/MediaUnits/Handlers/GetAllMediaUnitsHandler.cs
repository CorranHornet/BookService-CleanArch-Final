using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using BookService.Domain.Entities;
using Mapster;

namespace BookService.Application.MediaUnits.Handlers;

public class GetAllMediaUnitsHandler : IRequestHandler<GetAllMediaUnitsQuery, IEnumerable<MediaUnitResponseDTO>>
{
    private readonly IMediaUnitRepository _repo;
    public GetAllMediaUnitsHandler(IMediaUnitRepository repo) => _repo = repo;

    public async Task<IEnumerable<MediaUnitResponseDTO>> Handle(GetAllMediaUnitsQuery request, CancellationToken ct)
    {
        var units = await _repo.GetAll();

        // 2. Map the collection to an IEnumerable of the DTO
        return units.Adapt<IEnumerable<MediaUnitResponseDTO>>();
    }
}