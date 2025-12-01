using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;
using TaskFlow.Application.Features.Boards.Users.Commands.RemoveUserCommand;

namespace TaskFlow.Api.Controllers;

[Route("Boards/{boardId:guid}/users")]
[ApiController]
public class BoardUserController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddUserToBoard(
    [FromRoute] Guid boardId,
    [FromBody] AddUserRequest request)
    {
        var result = await mediator.Send(new AddUserCommand
        {
            BoardId = boardId,
            UserEmail = request.UserEmail
        });

        return Created(string.Empty, result);
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserFromBoard(
        [FromRoute] Guid boardId,
        [FromRoute] Guid userId)
    {
        await mediator.Send(new RemoveUserCommand()
        {
            BoardId = boardId,
            UserId = userId
        });

        return NoContent();
    }
}