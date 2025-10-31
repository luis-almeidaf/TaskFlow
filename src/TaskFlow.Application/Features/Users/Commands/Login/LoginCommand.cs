using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.Login;
public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
