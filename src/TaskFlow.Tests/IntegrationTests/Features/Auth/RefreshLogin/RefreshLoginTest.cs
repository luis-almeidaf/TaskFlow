using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Auth.Commands.RefreshTokenLogin;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Auth.RefreshLogin;

public class RefreshLoginTest : TaskFlowClassFixture
{
    private const string Route = "Auth/refresh-token";

    private readonly string _validRefreshToken;
    private readonly string _expiredRefreshToken;
    
    public RefreshLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _validRefreshToken = webApplicationFactory.RefreshTokenValid.GetRefreshToken();
        _expiredRefreshToken = webApplicationFactory.RefreshTokenExpired.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RefreshTokenLoginRequest { RefreshToken = _validRefreshToken };

        var response = await DoPost(Route, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Refresh_Token_Expired()
    {
        var request = new RefreshTokenLoginRequest { RefreshToken = _expiredRefreshToken };

        var response = await DoPost(Route, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And
            .Contain(error => error.GetString()!.Equals(ResourceErrorMessages.REFRESH_TOKEN_EXPIRED));
    }
}