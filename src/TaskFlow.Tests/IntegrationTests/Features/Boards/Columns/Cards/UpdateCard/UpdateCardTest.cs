using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.Cards.UpdateCard;

public class UpdateCardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly Guid _columnId;
    private readonly Guid _cardId;
    private readonly Guid _userToBeAssignedId;
    private readonly string _boardOwnerToken;
    private readonly string _boardGuestToken;

    public UpdateCardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columnId = webApplicationFactory.Column.GetId();
        _cardId = webApplicationFactory.Card.GetId();
        _userToBeAssignedId = webApplicationFactory.UserAdmin.GetId();
        _boardOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new UpdateCardRequest
        {
            Title = "New Title",
            Description = "New Description",
            AssignedToId = _userToBeAssignedId,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedCard = await DoGet($"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}", _boardOwnerToken);

        var updatedCardResponseBody = await updatedCard.Content.ReadAsStreamAsync();

        var updatedCardResponseData = await JsonDocument.ParseAsync(updatedCardResponseBody);

        updatedCardResponseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        updatedCardResponseData.RootElement.GetProperty("description").GetString().Should().Be(request.Description);
        updatedCardResponseData.RootElement.GetProperty("position").GetInt32().Should().BeGreaterThanOrEqualTo(0);
        updatedCardResponseData.RootElement.GetProperty("assignedTo").GetProperty("id").GetGuid().Should()
            .Be(request.AssignedToId!.Value);
        updatedCardResponseData.RootElement.GetProperty("dueDate").GetDateTime().Should().Be(request.DueDate);
    }

    [Fact]
    public async Task Error_Title_Cannot_Be_Empty()
    {
        var request = new UpdateCardRequest { Title = "", };

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.TITLE_CANNOT_BE_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_GuestTriesToUpdateCard()
    {
        var request = new UpdateCardRequest { Title = "New Title", };

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request,
            _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var request = new UpdateCardRequest { Title = "New Title", };

        var fakeColumnId = Guid.NewGuid();

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{fakeColumnId}/cards/{_cardId}",
            request,
            _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var request = new UpdateCardRequest { Title = "New Title", };

        var fakeCardId = Guid.NewGuid();

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{fakeCardId}",
            request,
            _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.CARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Use_To_Be_Assigned_Not_In_Board()
    {
        var userToBeAssignedId = Guid.NewGuid();

        var request = new UpdateCardRequest
        {
            Title = "New Title",
            AssignedToId = userToBeAssignedId
        };

        var response = await DoPut(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}",
            request,
            _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.USER_NOT_IN_BOARD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}