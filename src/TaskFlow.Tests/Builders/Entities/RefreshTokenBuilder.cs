using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build()
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "token",
            UserId = Guid.NewGuid(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };
    }
}