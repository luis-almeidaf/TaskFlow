using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
    string GenerateRefreshToken();
}