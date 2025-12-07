using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.DeleteCardCommand;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;
using TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;

namespace TaskFlow.Api.Controllers;

[Route("/Boards/{boardId:guid}/columns/{columnId:guid}/cards")]
[ApiController]
[Authorize]
public class BoardColumnCardController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateCardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCard(
        [FromRoute] Guid boardId,
        [FromRoute] Guid columnId,
        [FromBody] CreateCardRequest request)
    {
        var result = await mediator.Send(new CreateCardCommand
        {
            BoardId = boardId,
            ColumnId = columnId,
            Title = request.Title,
            Description = request.Description,
            AssignedToId = request.AssignedToId,
            DueDate = request.DueDate
        });

        return Created(string.Empty, result);
    }

    [HttpGet("{cardId:guid}")]
    [ProducesResponseType(typeof(GetCardByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCardById([FromRoute] Guid cardId, Guid boardId, Guid columnId)
    {
        var response = await mediator.Send(new GetCardByIdQuery()
        {
            BoardId = boardId,
            ColumnId = columnId,
            CardId = cardId
        });

        return Ok(response);
    }

    [HttpPut("{cardId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid boardId, Guid columnId, Guid cardId,
        [FromBody] UpdateCardRequest request)
    {
        await mediator.Send(new UpdateCardCommand
        {
            BoardId = boardId,
            ColumnId = columnId,
            CardId = cardId,
            Title = request.Title,
            Description = request.Description,
            AssignedToId = request.AssignedToId,
            DueDate = request.DueDate
        });

        return NoContent();
    }

    [HttpPatch("{cardId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Move(
        [FromRoute] Guid boardId, Guid columnId, Guid cardId,
        [FromBody] MoveCardRequest request)
    {
        await mediator.Send(new MoveCardCommand
        {
            BoardId = boardId,
            CurrentColumnId = columnId,
            CardId = cardId,
            NewColumnId = request.NewColumnId,
            NewPosition = request.NewPosition
        });

        return NoContent();
    }

    [HttpDelete("{cardId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid boardId, Guid columnId, Guid cardId)
    {
        await mediator.Send(new DeleteCardCommand
        {
            BoardId = boardId,
            ColumnId = columnId,
            CardId = cardId,
        });

        return NoContent();
    }
}