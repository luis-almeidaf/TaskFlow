using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Update;

public class UpdateBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _boardOwnerToken;
    private readonly string _boardGuestToken;
    private readonly Guid _boardId;

    public UpdateBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardOwnerToken = webApplicationFactory.UserOwner.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
        _boardId = webApplicationFactory.Board.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = new UpdateBoardRequest ("New board name");
        
        var response = await DoPatch(requestUri: $"{Route}/{_boardId}", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = new UpdateBoardRequest ("");
        
        var response = await DoPatch(requestUri: $"{Route}/{_boardId}", request: request, token: _boardOwnerToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.NAME_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_Guest_Tries_ToUpdateBoard()
    {
        var request = new UpdateBoardRequest ("New board name");
        
        var response = await DoPatch(requestUri: $"{Route}/{_boardId}", request: request, token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}