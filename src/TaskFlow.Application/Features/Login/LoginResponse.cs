namespace TaskFlow.Application.Features.Login;
public class LoginResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
