using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.GetAll;

public class GetBoardsTest: TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _userWithBoardsToken;
    private readonly string _userWithoutBoardsToken;

    public GetBoardsTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userWithBoardsToken = webApplicationFactory.UserWithBoards.GetToken();
        _userWithoutBoardsToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet($"{Route}", _userWithBoardsToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boards").EnumerateArray().Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task User_Without_Boards_Should_Return_Empty_List()
    {
        var response = await DoGet($"{Route}", _userWithoutBoardsToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boards").EnumerateArray().Should().BeEmpty();
    }
}