using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard.Requests;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.DeleteColumnFromBoard;

public class DeleteColumnFromBoardTest : TaskFlowClassFixture
{
    private const string Route = "Board";
    
    private readonly Guid _boardId;
    private readonly string _boardOwnerToken;

    public DeleteColumnFromBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddColumnToBoardRequest { Name = "New Column" };
        
        var response = await DoPost( $"{Route}/{_boardId}/columns", request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var columnId =responseData.RootElement.GetProperty("columnId").GetGuid();

        response = await DoDelete(requestUri: $"{Route}/{_boardId}/columns/{columnId}", token: _boardOwnerToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var fakeBoardId = Guid.NewGuid();
        
        var response = await DoDelete(requestUri: $"{Route}/{fakeBoardId}/columns/{fakeBoardId}", token: _boardOwnerToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()! == expectedMessage);

    }
    
    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var fakeColumnId = Guid.NewGuid();
        
        var response = await DoDelete(requestUri: $"{Route}/{_boardId}/columns/{fakeColumnId}", token: _boardOwnerToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}