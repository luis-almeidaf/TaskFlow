using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.DeleteColumn;

public class DeleteColumnTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly string _boardAdminToken;
    private readonly string _boardGuestToken;

    public DeleteColumnTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardAdminToken = webApplicationFactory.UserAdmin.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new CreateColumnRequest { Name = "New Column" };

        var response = await DoPost($"/{Route}/{_boardId}/columns", request, token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var columnId = responseData.RootElement.GetProperty("columnId").GetGuid();

        response = await DoDelete(requestUri: $"{Route}/{_boardId}/columns/{columnId}", token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnForbidden_When_GuestTriesToDeleteColumn()
    {
        var request = new CreateColumnRequest { Name = "New Column" };

        var response = await DoPost($"/{Route}/{_boardId}/columns", request, token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var columnId = responseData.RootElement.GetProperty("columnId").GetGuid();

        response = await DoDelete(requestUri: $"{Route}/{_boardId}/columns/{columnId}", token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var fakeColumnId = Guid.NewGuid();

        var response = await DoDelete(requestUri: $"/{Route}/{_boardId}/columns/{fakeColumnId}",
            token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.COLUMN_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}