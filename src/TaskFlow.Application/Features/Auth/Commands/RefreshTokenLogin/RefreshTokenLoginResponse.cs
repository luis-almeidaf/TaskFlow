namespace TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;

public class RefreshTokenLoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}