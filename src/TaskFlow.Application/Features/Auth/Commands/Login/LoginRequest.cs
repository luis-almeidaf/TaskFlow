namespace TaskFlow.Application.Features.Auth.Commands.Login;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}