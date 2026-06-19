using MediatR;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Application.MediaUnits.Queries;
using MapsterMapper;

namespace BookService.Application.MediaUnits.Handlers
{

    public class GetMediaUnitByIdHandler : IRequestHandler<GetMediaUnitByIdQuery, MediaUnitDTO?>
    {
        private readonly IMediaUnitRepository _repo;
        private readonly IMapper _mapper;
        public GetMediaUnitByIdHandler(IMediaUnitRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<MediaUnitDTO?> Handle(GetMediaUnitByIdQuery request, CancellationToken ct)
        {
            var mu = await _repo.GetById(request.Id);
            if (mu == null) return null;

            return _mapper.Map<MediaUnitDTO>(mu);
        }
    }
}