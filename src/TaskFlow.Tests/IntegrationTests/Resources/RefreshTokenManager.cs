namespace TaskFlow.Tests.IntegrationTests.Resources;

public class RefreshTokenManager(string refreshToken)
{
    public string GetRefreshToken() => refreshToken;

}