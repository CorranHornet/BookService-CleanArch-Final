// Location: BookService.Application/MediaItems/Queries/GetMediaItemsQuery.cs
using BookService.Application.Common.Interfaces;
using BookService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.MediaItems.Queries;

public record GetMediaItemsQuery : IRequest<List<MediaItem>>;

public class GetMediaItemsHandler : IRequestHandler<GetMediaItemsQuery, List<MediaItem>>
{
    private readonly IBookServiceDbContext _context;

    public GetMediaItemsHandler(IBookServiceDbContext context)
    {
        _context = context;
    }

    public async Task<List<MediaItem>> Handle(GetMediaItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.MediaItems
            .Include(m => m.Genre)
            .ToListAsync(cancellationToken);
    }
}