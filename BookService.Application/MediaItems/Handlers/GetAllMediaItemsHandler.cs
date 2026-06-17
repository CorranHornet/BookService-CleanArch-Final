using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using BookService.Application.Common.Diagnostics;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using BookService.Application.Interfaces;

namespace BookService.Application.MediaItems.Handlers
{
    public class GetAllMediaItemsHandler
        : IRequestHandler<GetAllMediaItemsQuery, List<MediaItemResponseDTO>>
    {
        private readonly IMediaItemRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllMediaItemsHandler> _logger;

        public GetAllMediaItemsHandler(
            IMediaItemRepository repo,
            IMapper mapper,
            ILogger<GetAllMediaItemsHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<MediaItemResponseDTO>> Handle(
            GetAllMediaItemsQuery request,
            CancellationToken cancellationToken)
        {
            AppLog.Info(_logger, "GetAllMediaItems started", new { request.Search });

            var items = await _repo.GetAll(request.Search);

            if (items == null)
            {
                AppLog.Warn(_logger, "Repository returned NULL");
                return new List<MediaItemResponseDTO>();
            }

            AppLog.Info(_logger, "Items from DB", new
            {
                Count = items.Count,
                Sample = items.FirstOrDefault()
            });

            var mapped = _mapper.Map<List<MediaItemResponseDTO>>(items);

            AppLog.Info(_logger, "Mapping completed", mapped);

            return mapped;
        }
    }
}