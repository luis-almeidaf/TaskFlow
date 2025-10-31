namespace TaskFlow.Application.Features.Users.Commands.Login;
public class LoginResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
