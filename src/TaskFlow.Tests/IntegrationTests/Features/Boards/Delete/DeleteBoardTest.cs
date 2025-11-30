using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Delete;

public class DeleteBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _token;
    private readonly Guid _boardId;

    public DeleteBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserWithBoards.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(requestUri: $"{Route}/{_boardId}", token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(requestUri: $"{Route}/{_boardId}", token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var fakeId = Guid.NewGuid();
        var response = await DoDelete(requestUri: $"{Route}/{fakeId}", token: _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}