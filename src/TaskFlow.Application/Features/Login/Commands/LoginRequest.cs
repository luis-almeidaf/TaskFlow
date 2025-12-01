namespace TaskFlow.Application.Features.Login.Commands;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}