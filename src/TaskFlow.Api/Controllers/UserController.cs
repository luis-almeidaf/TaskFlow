using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Users.Commands.ChangePasswordCommand;
using TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;
using TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;
using TaskFlow.Application.Features.Users.Commands.UpdateCommand;
using TaskFlow.Application.Features.Users.Queries.GetByEmailQuery;

namespace TaskFlow.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var response = await mediator.Send(new RegisterUserCommand
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            });

            return Created(string.Empty, response);
        }

        [HttpGet("{email}")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserByEmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var response = await mediator.Send(new GetUserByEmailQuery { Email = email });
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            await mediator.Send(new UpdateUserCommand
            {
                Email = request.Email,
                Name = request.Name
            });

            return NoContent();
        }

        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await mediator.Send(new ChangePasswordCommand
            {
                NewPassword = request.NewPassword,
                Password = request.Password
            });

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete()
        {
            await mediator.Send(new DeleteUserCommand());
            return NoContent();
        }
    }
}