using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookService.Application.MediaItems.Commands
{
    public class CreateMediaItemHandler
        : IRequestHandler<CreateMediaItemCommand, MediaItemResponseDTO>
    {
        private readonly IMediaItemRepository _repo;
        private readonly IMapper _mapper;

        public CreateMediaItemHandler(IMediaItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<MediaItemResponseDTO> Handle(
            CreateMediaItemCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Maps command to entity
            var entity = _mapper.Map<MediaItem>(request);

            
            // 2. Save in db (using repository)
            await _repo.Add(entity);
            await _repo.SaveChangesAsync();

            // 3. Return DTO
            return _mapper.Map<MediaItemResponseDTO>(entity);
        }
    }
}