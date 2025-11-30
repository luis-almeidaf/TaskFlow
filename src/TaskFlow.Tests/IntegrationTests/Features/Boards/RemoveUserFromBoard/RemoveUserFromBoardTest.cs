using FluentAssertions;
using System.Net;
using System.Text.Json;
using TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.RemoveUserFromBoard;

public class RemoveUserFromBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;

    private readonly Guid _boardOwnerId;
    private readonly string _boardOwnerToken;

    private readonly Guid _userId;
    private readonly string _userToBeAddedEmail;

    public RemoveUserFromBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerId = webApplicationFactory.UserWithBoards.GetId();
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
        _userId = webApplicationFactory.User.GetId();
        _userToBeAddedEmail = webApplicationFactory.User.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddUserRequest
        {
            UserEmail = _userToBeAddedEmail
        };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_userId}", token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var fakeBoardId = Guid.NewGuid();
        var response = await DoDelete(requestUri: $"/{Route}/{fakeBoardId}/users/{_userId}", token: _boardOwnerToken);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.BOARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var fakeUserId = Guid.NewGuid();

        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{fakeUserId}", token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.USER_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_User_Not_In_Board()
    {
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_userId}", token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.USER_NOT_IN_BOARD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Board_Owner_Cannot_Be_Removed()
    {
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_boardOwnerId}", token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.BOARD_OWNER_CANNOT_BE_REMOVED;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}