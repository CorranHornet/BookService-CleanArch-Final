using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using BookService.Domain.Entities;
using Mapster;

namespace BookService.Application.MediaUnits.Handlers;

public class GetMediaUnitByIdHandler : IRequestHandler<GetMediaUnitByIdQuery, MediaUnitResponseDTO?>
{
    private readonly IMediaUnitRepository _repo;
    public GetMediaUnitByIdHandler(IMediaUnitRepository repo) => _repo = repo;

    public async Task<MediaUnitResponseDTO?> Handle(GetMediaUnitByIdQuery request, CancellationToken ct)
    {
        var mu = await _repo.GetById(request.Id);
        if (mu == null) return null;

        return mu.Adapt<MediaUnitResponseDTO>();
    }
}