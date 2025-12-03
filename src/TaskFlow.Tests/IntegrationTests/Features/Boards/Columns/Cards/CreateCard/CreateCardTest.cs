using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.Cards.CreateCard;

public class CreateCardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly Guid _columnId;
    private readonly string _boardOwnerToken;

    public CreateCardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columnId = webApplicationFactory.Column.GetId();
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new CreateCardRequest { Title = "New Card" };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boardId").GetGuid().Should().Be(_boardId);
        responseData.RootElement.GetProperty("columnId").GetGuid().Should().Be(_columnId);
        responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var request = new CreateCardRequest { Title = "New Card" };

        var fakeId = Guid.NewGuid();

        var response = await DoPost(requestUri: $"/{Route}/{fakeId}/columns/{_columnId}/cards", request: request,
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
        var request = new CreateCardRequest { Title = "New Card" };

        var fakeId = Guid.NewGuid();

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/columns/{fakeId}/cards", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Assigned_User_Not_In_Board()
    {
        var request = new CreateCardRequest
        {
            Title = "New Card",
            AssignedToId = Guid.NewGuid()
        };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.USER_NOT_IN_BOARD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}