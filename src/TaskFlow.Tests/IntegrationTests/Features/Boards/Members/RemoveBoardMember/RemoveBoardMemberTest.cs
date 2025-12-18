using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Members.RemoveBoardMember;

public class RemoveBoardMemberTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;

    private readonly Guid _boardOwnerId;
    private readonly string _boardOwnerToken;
    private readonly string _boardGuestToken;

    private readonly Guid _userToBeAddedId;
    private readonly string _userToBeAddedEmail;

    public RemoveBoardMemberTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerId = webApplicationFactory.UserOwner.GetId();
        _boardOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
        _userToBeAddedId = webApplicationFactory.UserOutOfBoard.GetId();
        _userToBeAddedEmail = webApplicationFactory.UserOutOfBoard.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddBoardMemberRequest
        {
            UserEmail = _userToBeAddedEmail
        };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_userToBeAddedId}", token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Error_User_Not_In_Board()
    {
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_userToBeAddedId}", token: _boardOwnerToken);

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
    
    [Fact]
    public async Task Should_ReturnForbidden_When_Guest_Tries_ToRemoveBoardMember()
    {
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/users/{_boardOwnerId}", token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}