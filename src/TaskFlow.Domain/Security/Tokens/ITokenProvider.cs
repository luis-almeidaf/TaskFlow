namespace TaskFlow.Domain.Security.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}