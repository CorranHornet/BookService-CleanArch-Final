using MediatR;
using BookService.Application.DTOs;

namespace BookService.Application.MediaUnits.Queries;

public record GetAllMediaUnitsQuery() : IRequest<IEnumerable<MediaUnitResponseDTO>>;