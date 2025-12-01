using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.UpdateColumn;

public class UpdateColumnTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly List<Column> _columns;
    private readonly string _boardOwnerToken;

    public UpdateColumnTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
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

        var request = new UpdateColumnRequest { Name = "New name" };

        var response = await DoPatch(requestUri: $"{Route}/{_boardId}/columns/{columnId}", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedBoard = await DoGet(requestUri: $"{Route}/{_boardId}", token: _boardOwnerToken);

        var updatedBoardResponseBody = await updatedBoard.Content.ReadAsStreamAsync();

        var updatedBoardResponseData = await JsonDocument.ParseAsync(updatedBoardResponseBody);

        var updatedBoardColumns = updatedBoardResponseData.RootElement.GetProperty("columns");

        var updatedColumn = updatedBoardColumns.EnumerateArray()
            .FirstOrDefault(col => col.GetProperty("id").GetGuid() == columnId);

        var newNameColumn = updatedColumn.GetProperty("name").GetString();
        newNameColumn.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Name_Cannot_Be_Empty()
    {
        var columnToMove = _columns[1];

        var columnId = columnToMove.Id;

        var request = new UpdateColumnRequest { Name = "" };

        var response = await DoPatch(requestUri: $"{Route}/{_boardId}/columns/{columnId}", request, _boardOwnerToken);

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
        var columnToMove = _columns[1];

        var columnId = columnToMove.Id;

        var request = new UpdateColumnRequest { Name = "New name" };

        var fakeBoardId = Guid.NewGuid();

        var response = await DoPatch(requestUri: $"{Route}/{fakeBoardId}/columns/{columnId}", request,
            _boardOwnerToken);

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

        var request = new UpdateColumnRequest { Name = "New name" };

        var response = await DoPatch(requestUri: $"{Route}/{_boardId}/columns/{fakeColumnId}/position", request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}