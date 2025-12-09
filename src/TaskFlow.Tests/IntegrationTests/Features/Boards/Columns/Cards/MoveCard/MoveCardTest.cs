using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.Cards.MoveCard;

public class MoveCardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly Guid _columnId;
    private readonly Guid _cardId;

    private readonly List<Column> _columns;
    private readonly string _boardOwnerToken;

    public MoveCardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columnId = webApplicationFactory.Column.GetId();
        _cardId = webApplicationFactory.Card.GetId();
        _columns = webApplicationFactory.Board.GetColumns;
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success_Moving_Card_Between_Columns()
    {
        var newColumn = _columns[1];

        var request = new MoveCardRequest { NewColumnId = newColumn.Id };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var cardInNewColumn =
            await DoGet($"/{Route}/{_boardId}/columns/{newColumn.Id}/cards/{_cardId}", _boardOwnerToken);

        var cardInNewColumnResponseBody = await cardInNewColumn.Content.ReadAsStreamAsync();

        var cardInNewColumnResponseData = await JsonDocument.ParseAsync(cardInNewColumnResponseBody);

        cardInNewColumnResponseData.RootElement.GetProperty("position").GetInt32().Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task Success_Moving_Card_Inside_Same_Columns()
    {
        var createCardRequest = new CreateCardRequest { Title = "New Card" };

        var response = await DoPost(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards",
            request: createCardRequest,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var newCardId = responseData.RootElement.GetProperty("cardId").GetGuid();

        var moveCardRequest = new MoveCardRequest { NewPosition = 1 };

        response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{newCardId}",
            request: moveCardRequest,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var cardInNewColumn = await DoGet($"/{Route}/{_boardId}/columns/{_columnId}/cards/{newCardId}", _boardOwnerToken);

        var cardInNewColumnResponseBody = await cardInNewColumn.Content.ReadAsStreamAsync();

        var cardInNewColumnResponseData = await JsonDocument.ParseAsync(cardInNewColumnResponseBody);

        cardInNewColumnResponseData.RootElement.GetProperty("position").GetInt32().Should().Be(moveCardRequest.NewPosition);
    }


    [Fact]
    public async Task Error_NewPosition_Cannot_Be_Negative()
    {
        var request = new MoveCardRequest { NewPosition = -2 };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request: request,
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
        var request = new MoveCardRequest { NewPosition = 1 };

        var fakeBoardId = Guid.NewGuid();

        var response = await DoPatch(
            requestUri: $"/{Route}/{fakeBoardId}/columns/{_columnId}/cards/{_cardId}",
            request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Card_Current_Column_Not_Found()
    {
        var request = new MoveCardRequest { NewPosition = 1 };

        var fakeColumnId = Guid.NewGuid();

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{fakeColumnId}/cards/{_cardId}",
            request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Card_New_Column_Not_Found()
    {
        var request = new MoveCardRequest { NewColumnId = Guid.NewGuid() };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_columnId}",
            request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.CARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}