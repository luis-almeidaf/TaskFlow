using FluentAssertions;
using System.Net;
using System.Text.Json;
using TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.MoveColumn;

public class MoveColumnTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly List<Column> _columns;
    private readonly string _boardOwnerToken;

    public MoveColumnTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columns = webApplicationFactory.Board.GetColumns;
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var columnToMove = _columns[1];

        var columnId = columnToMove.Id;

        var request = new MoveColumnRequest { NewPosition = 2 };

        var response = await DoPatch(requestUri: $"/{Route}/{_boardId}/columns/{columnId}/position", request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedBoard = await DoGet(requestUri: $"{Route}/{_boardId}", token: _boardOwnerToken);

        var updatedBoardResponseBody = await updatedBoard.Content.ReadAsStreamAsync();

        var updatedBoardResponseData = await JsonDocument.ParseAsync(updatedBoardResponseBody);

        var updatedBoardColumns = updatedBoardResponseData.RootElement.GetProperty("columns");

        var movedColumn = updatedBoardColumns.EnumerateArray()
            .FirstOrDefault(col => col.GetProperty("id").GetGuid() == columnId);

        var newPosition = movedColumn.GetProperty("position").GetInt32();
        newPosition.Should().Be(2);
    }

    [Fact]
    public async Task Error_NewPosition_Cannot_Be_Negative()
    {
        var columnToMove = _columns[1];

        var columnId = columnToMove.Id;

        var request = new MoveColumnRequest { NewPosition = -1 };

        var response = await DoPatch(requestUri: $"/{Route}/{_boardId}/columns/{columnId}/position", request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.NEW_POSITION_CANNOT_BE_NEGATIVE;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var columnToMove = _columns[1];

        var columnId = columnToMove.Id;

        var request = new MoveColumnRequest { NewPosition = 2 };

        var fakeBoardId = Guid.NewGuid();

        var response = await DoPatch(requestUri: $"/{Route}/{fakeBoardId}/columns/{columnId}/position", request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var fakeColumnId = Guid.NewGuid();

        var request = new MoveColumnRequest { NewPosition = 2 };

        var response = await DoPatch(requestUri: $"/{Route}/{_boardId}/columns/{fakeColumnId}/position", request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}