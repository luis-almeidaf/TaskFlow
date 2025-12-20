namespace TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}