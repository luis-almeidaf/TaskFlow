namespace TaskFlow.Application.Features.Auth.Commands.Login;
public class LoginResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
