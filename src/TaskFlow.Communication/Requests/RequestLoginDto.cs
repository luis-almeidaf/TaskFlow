namespace TaskFlow.Communication.Requests;

public class RequestLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}