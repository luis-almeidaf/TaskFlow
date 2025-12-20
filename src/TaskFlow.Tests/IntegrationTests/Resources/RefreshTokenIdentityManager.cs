using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.IntegrationTests.Resources;

public class RefreshTokenIdentityManager(string refreshToken)
{
    public string GetRefreshToken() => refreshToken;

}