using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard.Requests;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.AddUserToBoard;

public class AddUserToBoardTest : TaskFlowClassFixture
{
    private const string Route = "Board";

    private readonly Guid _boardId;

    private readonly string _boardOwnerToken;
    private readonly string _boardOwnerEmail;

    private readonly string _userToBeAddedEmail;
    private readonly string _userToBeAddedName;

    public AddUserToBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
        _boardOwnerEmail = webApplicationFactory.UserWithBoards.GetEmail();
        _userToBeAddedEmail = webApplicationFactory.User.GetEmail();
        _userToBeAddedName = webApplicationFactory.User.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AddUserToBoardRequest
        {
            UserEmail = _userToBeAddedEmail
        };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_userToBeAddedName);
        responseData.RootElement.GetProperty("email").GetString().Should().Be(_userToBeAddedEmail);
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = new AddUserToBoardRequest
        {
            UserEmail = string.Empty
        };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.EMAIL_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var request = new AddUserToBoardRequest
        {
            UserEmail = _userToBeAddedEmail
        };

        var fakeId = Guid.NewGuid();
        var response = await DoPost(requestUri: $"{Route}/{fakeId}/users", request: request, token: _boardOwnerToken);

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
        var request = new AddUserToBoardRequest
        {
            UserEmail = "fakeemail@email.com"
        };

        var response = await DoPost(requestUri: $"{Route}/{_boardId}/users", request: request, token: _boardOwnerToken);

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
        var request = new AddUserToBoardRequest
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
}