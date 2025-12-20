using System.Net;
using FluentAssertions;

namespace TaskFlow.Tests.IntegrationTests.Features.Auth.Logout;

public class LogoutTest : TaskFlowClassFixture
{
    private const string Route = "Auth/logout";
    private readonly string _userToken;
    
    public LogoutTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userToken = webApplicationFactory.UserGuest.GetToken();
    }
    
    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(requestUri: $"{Route}", token: _userToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}