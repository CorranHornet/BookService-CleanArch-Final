using BookService.Infrastructure.Persistence;
using BookService.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookService.Application.MediaItems.Queries.GetAllMediaItems;

public class GetAllMediaItemsHandler
{
    private readonly AppDbContext _context;

    public GetAllMediaItemsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MediaItemResponseDTO>> Handle(GetAllMediaItemsQuery query)
    {
        var items = _context.MediaItems
            .Include(m => m.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            items = items.Where(x =>
                x.Title != null &&
                x.Title.ToLower().Contains(query.Search.ToLower()));
        }

        return await items.Select(t => new MediaItemResponseDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            GenreId = t.GenreId,
            Genre = t.Genre.Name,
            Creator = t.Creator,
            ReleaseDate = t.ReleaseDate,
            ScheduledDate = t.ScheduledDate,
            PageCount = t.PageCount,
            DurationMinutes = t.DurationMinutes,
            TrackCount = t.TrackCount,
            Publisher = t.Publisher,
            Language = t.Language,
            MediaType = t.MediaType
        }).ToListAsync();
    }
}