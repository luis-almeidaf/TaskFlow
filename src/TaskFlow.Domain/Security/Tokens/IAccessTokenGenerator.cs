using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}