using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard.Requests;
using TaskFlow.Application.Features.Boards.Commands.Create;
using TaskFlow.Application.Features.Boards.Commands.GetAll;
using TaskFlow.Application.Features.Boards.Commands.GetByID;

namespace TaskFlow.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBoardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateBoardCommand request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetBoardsCommand());
        
        if (response.Boards.Count != 0)
            return Ok(response);

        return NoContent();
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetBoardByCommand {Id = id});

        return Ok(response);
    }
    
    [HttpPost]
    [Route("{boardId:guid}/users")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddUserToBoard(
        [FromRoute] Guid boardId,
        [FromBody] AddUserToBoardRequest request)
    {
        await _mediator.Send(new AddUserToBoardCommand
            {
                BoardId = boardId, 
                UserEmail = request.UserEmail
            });

        return NoContent();
    }
}
