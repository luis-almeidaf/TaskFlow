using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;
using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard.Requests;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard.Requests;
using TaskFlow.Application.Features.Boards.Commands.Create;
using TaskFlow.Application.Features.Boards.Commands.Delete;
using TaskFlow.Application.Features.Boards.Commands.DeleteColumnFromBoard;
using TaskFlow.Application.Features.Boards.Commands.DeleteUserFromBoard;
using TaskFlow.Application.Features.Boards.Commands.GetAll;
using TaskFlow.Application.Features.Boards.Commands.GetById;
using TaskFlow.Application.Features.Boards.Commands.MoveColumn;
using TaskFlow.Application.Features.Boards.Commands.MoveColumn.Request;
using TaskFlow.Application.Features.Boards.Commands.Update;
using TaskFlow.Application.Features.Boards.Commands.Update.Requests;
using TaskFlow.Application.Features.Boards.Commands.UpdateColumn;
using TaskFlow.Application.Features.Boards.Commands.UpdateColumn.Request;

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


    [HttpPost]
    [Route("{boardId:guid}/users")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddUserToBoard(
        [FromRoute] Guid boardId,
        [FromBody] AddUserToBoardRequest request)
    {
        var result = await mediator.Send(new AddUserToBoardCommand
        {
            BoardId = boardId,
            UserEmail = request.UserEmail
        });

        return Created(string.Empty, result);
    }

    [HttpDelete]
    [Route("{boardId:guid}/users/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserFromBoard(
        [FromRoute] Guid boardId,
        [FromRoute] Guid userId)
    {
        await mediator.Send(new DeleteUserFromBoardCommand()
        {
            BoardId = boardId,
            UserId = userId
        });

        return NoContent();
    }

    [HttpPost]
    [Route("{boardId:guid}/columns")]
    [ProducesResponseType(typeof(AddColumnToBoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddColumnToBoard(
        [FromRoute] Guid boardId,
        [FromBody] AddColumnToBoardRequest toBoardRequest)
    {
        var result = await mediator.Send(new AddColumnToBoardCommand
        {
            BoardId = boardId,
            Name = toBoardRequest.Name
        });

        return Created(string.Empty, result);
    }

    [HttpDelete]
    [Route("{boardId:guid}/columns/{columnId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteColumnFromBoard(
        [FromRoute] Guid boardId,
        [FromRoute] Guid columnId)
    {
        await mediator.Send(new DeleteColumnFromBoardCommand
        {
            BoardId = boardId,
            ColumnId = columnId
        });

        return NoContent();
    }

    [HttpPatch]
    [Route("{boardId:guid}/columns/{columnId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
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

    [HttpPatch]
    [Route("{boardId:guid}/columns/{columnId:guid}/position")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
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