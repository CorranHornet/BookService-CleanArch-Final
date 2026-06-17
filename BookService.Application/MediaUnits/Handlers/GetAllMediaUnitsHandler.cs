using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using MapsterMapper;

namespace BookService.Application.MediaUnits.Handlers;

public class GetAllMediaUnitsHandler : IRequestHandler<GetAllMediaUnitsQuery, IEnumerable<MediaUnitDTO>>
{
    private readonly IMediaUnitRepository _repo;
    private readonly IMapper _mapper;
    public GetAllMediaUnitsHandler(IMediaUnitRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<IEnumerable<MediaUnitDTO>> Handle(GetAllMediaUnitsQuery request, CancellationToken ct)
    {
        var units = await _repo.GetAll();

        // 2. Map the collection to an IEnumerable of the DTO
        return _mapper.Map<IEnumerable<MediaUnitDTO>>(units);
    }
}