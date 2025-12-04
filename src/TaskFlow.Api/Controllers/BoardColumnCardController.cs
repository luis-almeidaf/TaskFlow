using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
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
}