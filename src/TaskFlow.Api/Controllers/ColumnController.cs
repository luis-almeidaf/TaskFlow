using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.GetById;
using TaskFlow.Application.Features.Columns.Create;
using TaskFlow.Application.Features.Columns.Create.Requests;

namespace TaskFlow.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ColumnController(IMediator mediator) : ControllerBase
{
    [HttpPost("{boardId:guid}")]
    [ProducesResponseType(typeof(CreateColumnResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid boardId,
        [FromBody] CreateColumnRequest request)
    {
        var result = await mediator.Send(new CreateColumnCommand
        {
            BoardId = boardId,
            Name = request.Name
        });

        return Created(string.Empty, result);
    }
}