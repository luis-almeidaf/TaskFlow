using System.Net;
using FluentAssertions;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Delete;

public class DeleteBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _userOwnerToken;
    private readonly string _userAdminToken;
    private readonly Guid _boardId;

    public DeleteBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _userAdminToken = webApplicationFactory.UserAdmin.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(requestUri: $"{Route}/{_boardId}", token: _userOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(requestUri: $"{Route}/{_boardId}", token: _userOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_NonOwner_Tries_ToDeleteBoard()
    {
        var response = await DoDelete(requestUri: $"{Route}/{_boardId}", token: _userAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}