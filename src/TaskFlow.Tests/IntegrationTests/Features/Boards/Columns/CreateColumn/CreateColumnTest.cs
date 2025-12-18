using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.CreateColumn;

public class CreateColumnTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly string _boardAdminToken;
    private readonly string _boardGuestToken;

    public CreateColumnTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _boardAdminToken = webApplicationFactory.UserAdmin.GetToken();
        _boardGuestToken = webApplicationFactory.UserGuest.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new CreateColumnRequest { Name = "New Column" };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/columns", request: request,
            token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("boardId").GetGuid().Should().NotBeEmpty();
        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = new CreateColumnRequest { Name = "" };

        var response = await DoPost(requestUri: $"/{Route}/{_boardId}/columns", request: request,
            token: _boardAdminToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.NAME_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Fact]
    public async Task Should_ReturnForbidden_When_GuestTriesToCreateColumn()
    {
        var request = new CreateColumnRequest { Name = "New Column" };

        var response = await DoPost(
            requestUri: $"/{Route}/{_boardId}/columns", 
            request: request,
            token: _boardGuestToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}