using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Users.Commands.Delete;
using TaskFlow.Application.Features.Users.Commands.GetByEmail;
using TaskFlow.Application.Features.Users.Commands.Register;
using TaskFlow.Application.Features.Users.Commands.Update;

namespace TaskFlow.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var response = await _mediator.Send(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var response = await _mediator.Send(new GetUserByEmailCommand { Email = email });
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return NoContent();
        }
    }
}