using MediatR;
using BookService.Application.DTOs;

namespace BookService.Application.MediaUnits.Commands
{
    public class CreateMediaUnitCommand : IRequest<MediaUnitDTO?>
    {
        public string Title { get; set; } = string.Empty;
        public int Number { get; set; }
        public int MediaItemId { get; set; }
        public int? DurationMinutes { get; set; }
        public int? PageCount { get; set; }
    }
}