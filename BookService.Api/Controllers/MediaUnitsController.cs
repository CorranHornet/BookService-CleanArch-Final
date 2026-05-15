using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookService.Application.MediaUnits.Commands;
using BookService.Application.MediaUnits.Queries;
using BookService.Application.MediaUnits.Handlers;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaUnitsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MediaUnitsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll() 
        => Ok(await _mediator.Send(new GetAllMediaUnitsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetMediaUnitByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaUnitCommand command)
    {
        var result = await _mediator.Send(command);
        return result != null ? Ok(result) : NotFound($"MediaItem {command.MediaItemId} not found.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMediaUnitCommand command)
    {
        command.Id = id;
        return await _mediator.Send(command) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _mediator.Send(new DeleteMediaUnitCommand(id)) ? NoContent() : NotFound();
    }
}