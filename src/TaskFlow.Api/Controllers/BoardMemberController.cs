using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Authorization;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;
using TaskFlow.Application.Features.Boards.Members.Commands.RemoveBoardMemberCommand;

namespace TaskFlow.Api.Controllers;

[Route("Boards/{boardId:guid}/users")]
[ApiController]
public class BoardMemberController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
    [Authorize(Policy = TaskFlowPermissions.Boards.AddBoardMember)]
    public async Task<IActionResult> AddBoardMember(
    [FromRoute] Guid boardId,
    [FromBody] AddBoardMemberRequest request)
    {
        var result = await mediator.Send(new AddBoardMemberCommand
        {
            BoardId = boardId,
            UserEmail = request.UserEmail,
            Role = request.Role
        });

        return Created(string.Empty, result);
    }

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = TaskFlowPermissions.Boards.RemoveBoardMember)]
    public async Task<IActionResult> RemoveBoardMember(
        [FromRoute] Guid boardId,
        [FromRoute] Guid userId)
    {
        await mediator.Send(new RemoveBoardMemberCommand()
        {
            BoardId = boardId,
            BoardMemberId = userId
        });

        return NoContent();
    }
}