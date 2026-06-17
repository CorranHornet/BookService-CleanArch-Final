using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.MediaUnits.Commands;
using BookService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookService.Application.MediaUnits.Handlers;

public class CreateMediaUnitHandler : IRequestHandler<CreateMediaUnitCommand, MediaUnitDTO>
{
    private readonly IMediaUnitRepository _repo;
    private readonly IMapper _mapper;

    public CreateMediaUnitHandler(IMediaUnitRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<MediaUnitDTO> Handle(CreateMediaUnitCommand request, CancellationToken ct)
    {
        if (request.PageCount.HasValue && request.DurationMinutes.HasValue)
            throw new ArgumentException("Cannot be both Book and Audiobook");

        if (!request.PageCount.HasValue && !request.DurationMinutes.HasValue)
            throw new ArgumentException("Must specify either PageCount or DurationMinutes");

        MediaUnit entity;

        if (request.PageCount.HasValue)
        {
            entity = _mapper.Map<PhysicalBookUnit>(request);
        }
        else
        {
            entity = _mapper.Map<AudiobookUnit>(request);
        }

        await _repo.Add(entity);
        await _repo.SaveChangesAsync();

        return _mapper.Map<MediaUnitDTO>(entity);
    }
}