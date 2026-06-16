using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.MediaUnits.Commands;
using BookService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookService.Application.MediaUnits.Handlers;

public class CreateMediaUnitHandler : IRequestHandler<CreateMediaUnitCommand, MediaUnitResponseDTO>
{
    private readonly IMediaUnitRepository _repo;
    private readonly IMapper _mapper;

    public CreateMediaUnitHandler(IMediaUnitRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<MediaUnitResponseDTO> Handle(CreateMediaUnitCommand request, CancellationToken ct)
    {
        // 1. Create entity based on input data(polymorphism)
        MediaUnit entity = (request.DurationMinutes.HasValue && request.DurationMinutes.Value > 0)
            ? _mapper.Map<AudiobookUnit>(request)
            : _mapper.Map<PhysicalBookUnit>(request);

        // 2. Save
        await _repo.Add(entity);
        await _repo.SaveChangesAsync();

        // 3. Return response
        return _mapper.Map<MediaUnitResponseDTO>(entity);
    }
}