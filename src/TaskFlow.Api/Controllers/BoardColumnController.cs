using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Authorization;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;
using TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;
using TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;

namespace TaskFlow.Api.Controllers;

[Route("/Boards/{boardId:guid}/columns")]
[ApiController]
[Authorize]
public class BoardColumnController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateColumnResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = TaskFlowPermissions.Columns.Create)]
    public async Task<IActionResult> CreateColumn(
    [FromRoute] Guid boardId,
    [FromBody] CreateColumnRequest toBoardRequest)
    {
        var result = await mediator.Send(new CreateColumnCommand
        {
            BoardId = boardId,
            Name = toBoardRequest.Name
        });

        return Created(string.Empty, result);
    }

    [HttpDelete("{columnId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = TaskFlowPermissions.Columns.Delete)]
    public async Task<IActionResult> DeleteColumn(
        [FromRoute] Guid boardId,
        [FromRoute] Guid columnId)
    {
        await mediator.Send(new DeleteColumnCommand
        {
            BoardId = boardId,
            ColumnId = columnId
        });

        return NoContent();
    }

    [HttpPatch("{columnId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = TaskFlowPermissions.Columns.Update)]
    public async Task<IActionResult> UpdateColumn(
        [FromRoute] Guid boardId,
        [FromRoute] Guid columnId,
        [FromBody] UpdateColumnRequest request)
    {
        await mediator.Send(new UpdateColumnCommand
        {
            BoardId = boardId,
            ColumnId = columnId,
            Name = request.Name
        });

        return NoContent();
    }

    [HttpPatch("{columnId:guid}/position")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = TaskFlowPermissions.Columns.Move)]
    public async Task<IActionResult> MoveColumn(
        [FromRoute] Guid boardId,
        [FromRoute] Guid columnId,
        [FromBody] MoveColumnRequest request)
    {
        await mediator.Send(new MoveColumnCommand()
        {
            BoardId = boardId,
            ColumnId = columnId,
            NewPosition = request.NewPosition
        });

        return NoContent();
    }
}