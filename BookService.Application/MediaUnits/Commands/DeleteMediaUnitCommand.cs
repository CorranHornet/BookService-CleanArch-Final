using MediatR;

namespace BookService.Application.MediaUnits.Commands;

public record DeleteMediaUnitCommand(int Id) : IRequest<bool>;