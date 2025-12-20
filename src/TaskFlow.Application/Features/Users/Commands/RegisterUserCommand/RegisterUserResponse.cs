namespace TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;

public class RegisterUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}