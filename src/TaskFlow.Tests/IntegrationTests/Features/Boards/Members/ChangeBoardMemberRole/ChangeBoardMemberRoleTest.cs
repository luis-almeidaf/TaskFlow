using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.ChangeBoardMemberRoleCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Members.ChangeBoardMemberRole;

public class ChangeBoardMemberRoleTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;

    private readonly Guid _userOwnerId;
    private readonly string _boardGuestToken;
    private readonly string _boardAdminToken;

    public ChangeBoardMemberRoleTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _userOwnerId = webApplicationFactory.UserOwner.GetId();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
        _boardAdminToken = webApplicationFactory.UserAdmin.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new ChangeBoardMemberRoleRequest() { Role = BoardRole.Guest };

        var response = await DoPatch(requestUri: $"/{Route}/{_boardId}/members/{_userOwnerId}", request: request,
            token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_User_Not_In_Board()
    {
        var fakeUserId = Guid.NewGuid();
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/members/{fakeUserId}", token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.USER_NOT_IN_BOARD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Fact]
    public async Task Should_ReturnForbidden_When_Guest_Tries_ToChangeBoardMemberRole()
    {
        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/members/{_userOwnerId}", token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}