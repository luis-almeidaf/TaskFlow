using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Boards.Commands.Create;

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
}