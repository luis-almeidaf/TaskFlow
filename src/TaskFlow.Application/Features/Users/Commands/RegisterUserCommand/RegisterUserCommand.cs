using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}