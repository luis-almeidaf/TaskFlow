using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.AssignUserCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.Cards.AssignUser;

public class AssignUserTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly Guid _columnId;
    private readonly Guid _cardId;
    private readonly Guid _userToBeAssignedId;
    private readonly string _boardOwnerToken;
    private readonly string _boardGuestToken;

    public AssignUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columnId = webApplicationFactory.Column.GetId();
        _cardId = webApplicationFactory.Card.GetId();
        _userToBeAssignedId = webApplicationFactory.UserGuest.GetId();
        _boardOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AssignUserRequest() { AssignedToId = _userToBeAssignedId };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}/assign",
            request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet($"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}", _boardOwnerToken);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("assignedTo").GetProperty("id").GetGuid().Should()
            .Be(request.AssignedToId);
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_GuestTriesToAssignUser()
    {
        var request = new AssignUserRequest() { AssignedToId = _userToBeAssignedId };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}/assign",
            request: request,
            token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var request = new AssignUserRequest() { AssignedToId = _userToBeAssignedId };

        var fakeColumnId = Guid.NewGuid();

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{fakeColumnId}/cards/{_cardId}/assign",
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
        var request = new AssignUserRequest() { AssignedToId = _userToBeAssignedId };

        var fakeCarId = Guid.NewGuid();

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{fakeCarId}/assign",
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
    public async Task Error_User_To_Be_Assigned_Not_In_Board()
    {
        var userToBeAssignedId = Guid.NewGuid();

        var request = new AssignUserRequest() { AssignedToId = userToBeAssignedId };

        var response = await DoPatch(
            requestUri: $"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}/assign",
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