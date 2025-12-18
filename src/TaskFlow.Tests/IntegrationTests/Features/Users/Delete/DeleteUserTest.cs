using System.Net;
using FluentAssertions;
using TaskFlow.Application.Features.Login.Commands;

namespace TaskFlow.Tests.IntegrationTests.Features.Users.Delete;

public class DeleteUserTest : TaskFlowClassFixture
{
    private const string Route = "User";
    
    private readonly string _userToken;
    private readonly string _userWithBoardsToken;
    private readonly string _email;
    private readonly string _password;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userToken = webApplicationFactory.UserGuest.GetToken();
        _userWithBoardsToken = webApplicationFactory.UserOwner.GetToken();
        _email = webApplicationFactory.UserGuest.GetEmail();
        _password = webApplicationFactory.UserGuest.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: Route, token: _userToken);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var request = new LoginRequest()
        {
            Email = _email,
            Password = _password
        };

        result = await DoPost(requestUri: "login", request);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_User_With_Boards()
    {
        var result = await DoDelete(requestUri: Route, token: _userWithBoardsToken);

        result.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}