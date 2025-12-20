using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.GetBoards;

public class GetBoardsTest: TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _userOwnerToken;
    private readonly string _userWithoutBoardToken;

    public GetBoardsTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _userWithoutBoardToken = webApplicationFactory.UserOutOfBoard.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet($"{Route}", _userOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boards").EnumerateArray().Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task User_Without_Boards_Should_Return_Empty_List()
    {
        var response = await DoGet($"{Route}", _userWithoutBoardToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boards").EnumerateArray().Should().BeEmpty();
    }
}