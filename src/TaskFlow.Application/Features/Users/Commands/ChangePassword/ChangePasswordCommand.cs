using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Unit>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}