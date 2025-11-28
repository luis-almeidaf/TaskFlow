using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard.Requests;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.AddColumnToBoard;

public class AddColumnToBoardTest : TaskFlowClassFixture
{
    private const string Route = "Board";

    private readonly Guid _boardId;
    private readonly string _boardOwnerToken;

    public AddColumnToBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddColumnToBoardRequest { Name = "New Column" };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/columns", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boardId").GetGuid().Should().NotBeEmpty();
        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = new AddColumnToBoardRequest { Name = "" };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/columns", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.NAME_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var request = new AddColumnToBoardRequest { Name = "New Column" };

        var fakeId = Guid.NewGuid();

        var response = await DoPost(requestUri: $"{Route}/{fakeId}/columns", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}