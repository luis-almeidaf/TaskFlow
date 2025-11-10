using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace TaskFlow.Tests.IntegrationTests.Features.Users.GetByEmail;

public class GetByEmailTest : TaskFlowClassFixture
{
    private const string Route = "User";
    
    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;
    
    public GetByEmailTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _userEmail = webApplicationFactory.User.GetEmail();
        _userName = webApplicationFactory.User.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet($"{Route}/{_userEmail}", _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        responseData.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var result = await DoGet($"{Route}/test@email", _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}