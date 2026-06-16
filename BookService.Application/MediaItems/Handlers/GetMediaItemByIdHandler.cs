using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using MapsterMapper;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class GetMediaItemByIdHandler
        : IRequestHandler<GetMediaItemByIdQuery, MediaItemResponseDTO?>
    {
        private readonly IMediaItemRepository _repo;
        private readonly IMapper _mapper;

        public GetMediaItemByIdHandler(IMediaItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<MediaItemResponseDTO?> Handle(
            GetMediaItemByIdQuery request,
            CancellationToken cancellationToken)
        {
            var item = await _repo.GetById(request.Id);

            if (item == null)
                return null;

            return _mapper.Map<MediaItemResponseDTO>(item);
        }
    }
}