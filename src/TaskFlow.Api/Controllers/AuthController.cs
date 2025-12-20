using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Auth.Commands.Login;
using TaskFlow.Application.Features.Auth.Commands.Logout;
using TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

namespace TaskFlow.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("login/")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await mediator.Send(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(RefreshTokenLoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginRequest request)
        {
            var response = await mediator.Send(new RefreshTokenLoginCommand
            {
                RefreshToken = request.RefreshToken
            });

            return Ok(response);
        }

        [HttpDelete("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            await mediator.Send(new LogoutCommand());
            return NoContent();
        }
    }
}