using MediatR;

namespace TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginCommand : IRequest<RefreshTokenLoginResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}