using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.UseCases.User.Register;
using TaskFlow.Communication.Requests;
using TaskFlow.Communication.Responses;

namespace TaskFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase userCase,
            [FromBody] RequestRegisterUserDto request)
        {
            var response = await userCase.Execute(request);

            return Created(string.Empty, response);

        }
    }
}
