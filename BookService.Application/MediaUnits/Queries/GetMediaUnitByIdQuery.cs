using MediatR;
using BookService.Application.DTOs;

namespace BookService.Application.MediaUnits.Queries
{

    public record GetMediaUnitByIdQuery(int Id) : IRequest<MediaUnitDTO?>;
}