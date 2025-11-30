using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Update;

public class UpdateBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _token;
    private readonly Guid _boardId;

    public UpdateBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserWithBoards.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var request = UpdateBoardCommandBuilder.Build(board);

        var response = await DoPatch(requestUri: $"{Route}/{_boardId}", request: request, token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var request = UpdateBoardCommandBuilder.Build(board);
        request.Name = string.Empty;

        var response = await DoPatch(requestUri: $"{Route}/{_boardId}", request: request, token: _token);

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
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var request = UpdateBoardCommandBuilder.Build(board);

        var fakeId = Guid.NewGuid();

        var response = await DoPatch(requestUri: $"{Route}/{fakeId}", request: request, token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}