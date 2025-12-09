using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common.Responses;
using TaskFlow.Application.Features.Login.Commands;

namespace TaskFlow.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
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
    }
}