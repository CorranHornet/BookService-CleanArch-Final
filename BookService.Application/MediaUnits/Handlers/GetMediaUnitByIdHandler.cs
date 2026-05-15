using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using BookService.Domain.Entities;

namespace BookService.Application.MediaUnits.Handlers;

public class GetMediaUnitByIdHandler : IRequestHandler<GetMediaUnitByIdQuery, MediaUnitResponseDTO?>
{
    private readonly IMediaUnitRepository _repo;
    public GetMediaUnitByIdHandler(IMediaUnitRepository repo) => _repo = repo;

    public async Task<MediaUnitResponseDTO?> Handle(GetMediaUnitByIdQuery request, CancellationToken ct)
    {
        var mu = await _repo.GetById(request.Id);
        if (mu == null) return null;

        return new MediaUnitResponseDTO {
            Id = mu.Id,
            Title = mu.Title,
            Number = mu.Number,
            MediaItemId = mu.MediaItemId,
            UnitType = mu is PhysicalBookUnit ? "Book" : "Audiobook",
            PageCount = mu is PhysicalBookUnit b ? b.PageCount : null,
            DurationMinutes = mu is AudiobookUnit a ? a.DurationMinutes : null
        };
    }
}