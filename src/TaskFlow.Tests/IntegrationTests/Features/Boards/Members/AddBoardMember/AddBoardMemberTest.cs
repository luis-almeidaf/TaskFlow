using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Members.AddBoardMember;

public class AddBoardMemberTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;

    private readonly string _boardOwnerToken;
    private readonly string _boardGuestToken;
    private readonly string _boardOwnerEmail;

    private readonly string _userToBeAddedEmail;
    private readonly string _userToBeAddedName;

    public AddBoardMemberTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerEmail = webApplicationFactory.UserOwner.GetEmail();
        _userToBeAddedEmail = webApplicationFactory.UserOutOfBoard.GetEmail();
        _userToBeAddedName = webApplicationFactory.UserOutOfBoard.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddBoardMemberRequest { UserEmail = _userToBeAddedEmail };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_userToBeAddedName);
        responseData.RootElement.GetProperty("email").GetString().Should().Be(_userToBeAddedEmail);
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = new AddBoardMemberRequest { UserEmail = string.Empty };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.EMAIL_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = new AddBoardMemberRequest
        {
            UserEmail = "fakeemail@email.com"
        };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request,
            token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.USER_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_User_Already_In_Board()
    {
        var request = new AddBoardMemberRequest
        {
            UserEmail = _boardOwnerEmail
        };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.USER_ALREADY_IN_BOARD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_Guest_Tries_ToAddBoardMember()
    {
        var request = new AddBoardMemberRequest { UserEmail = _userToBeAddedEmail };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/users", request: request,
            token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}