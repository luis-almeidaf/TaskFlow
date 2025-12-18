using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Create;

public class CreateBoardTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly string _token;

    public CreateBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserGuest.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new CreateBoardRequest { Name = "New Board" };

        var result = await DoPost(Route, request, _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrEmpty();
        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = new CreateBoardRequest { Name = "" };

        var result = await DoPost(Route, request, _token);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}