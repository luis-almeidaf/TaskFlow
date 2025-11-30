using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.ChangePasswordCommand;

public class ChangePasswordCommand : IRequest<Unit>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}