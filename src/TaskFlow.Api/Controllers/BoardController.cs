using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;
using TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;
using TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;
using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery;
using TaskFlow.Application.Features.Boards.Queries.GetByIdBoardQuery;

namespace TaskFlow.Api.Controllers;

[Route("/Boards")]
[ApiController]
[Authorize]
public class BoardController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateBoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBoardRequest request)
    {
        var response = await mediator.Send(new CreateBoardCommand { Name = request.Name });

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetBoardsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBoards()
    {
        var response = await mediator.Send(new GetBoardsQuery());

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetBoardByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await mediator.Send(new GetBoardByIdQuery { Id = id });

        return Ok(response);
    }

    [HttpPatch("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid boardId,
        [FromBody] UpdateBoardRequest request)
    {
        await mediator.Send(new UpdateBoardCommand
        {
            Id = boardId,
            Name = request.Name,
        });

        return NoContent();
    }

    [HttpDelete("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid boardId)
    {
        await mediator.Send(new DeleteBoardCommand { Id = boardId });
        return NoContent();
    }
}