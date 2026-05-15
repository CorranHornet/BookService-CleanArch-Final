using MediatR;

namespace BookService.Application.MediaUnits.Commands;

public class UpdateMediaUnitCommand : IRequest<bool>
{
    public int Id { get; set; } // Set from the URL in the Controller
    public string? Title { get; set; }
    public int? Number { get; set; }
    public int? PageCount { get; set; }
    public int? DurationMinutes { get; set; }
}