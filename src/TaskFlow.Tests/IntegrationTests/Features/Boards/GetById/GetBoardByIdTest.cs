using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.GetById;

public class GetBoardByIdTest : TaskFlowClassFixture
{
    private const string Route = "Board";

    private readonly string _token;
    private readonly Guid _boardId;

    public GetBoardByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserWithBoards.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet($"{Route}/{_boardId}", _token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetGuid().Should().Be(_boardId);
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Board_Not_Found()
    {
        var fakeId = Guid.NewGuid();
        var response = await DoGet($"{Route}/{fakeId}", _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}