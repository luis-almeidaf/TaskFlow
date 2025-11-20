using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard.Requests;
using TaskFlow.Application.Features.Boards.Commands.Create;
using TaskFlow.Application.Features.Boards.Commands.Delete;
using TaskFlow.Application.Features.Boards.Commands.GetAll;
using TaskFlow.Application.Features.Boards.Commands.GetById;
using TaskFlow.Application.Features.Boards.Commands.RemoveUserFromBoard;
using TaskFlow.Application.Features.Boards.Commands.Update;
using TaskFlow.Application.Features.Boards.Commands.Update.Requests;

namespace TaskFlow.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BoardController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateBoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBoardCommand request)
    {
        var response = await mediator.Send(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetBoardsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await mediator.Send(new GetBoardsCommand());
        
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(GetBoardByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await mediator.Send(new GetBoardByIdCommand { Id = id });

        return Ok(response);
    }

    [HttpPost]
    [Route("{boardId:guid}/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddUserToBoard(
        [FromRoute] Guid boardId,
        [FromBody] AddUserToBoardRequest request)
    {
        var result =await mediator.Send(new AddUserToBoardCommand
        {
            BoardId = boardId,
            UserEmail = request.UserEmail
        });

        return Ok(result);
    }

    [HttpDelete]
    [Route("{boardId:guid}/users/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveUserFromBoard(
        [FromRoute] Guid boardId,
        [FromRoute] Guid userId)
    {
        await mediator.Send(new RemoveUserFromBoardCommand()
        {
            BoardId = boardId,
            UserId = userId
        });

        return NoContent();
    }

    [HttpPatch]
    [Route("{boardId:guid}")]
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

    [HttpDelete]
    [Route("{boardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid boardId)
    {
        await mediator.Send(new DeleteBoardCommand { Id = boardId });
        return NoContent();
    }
}