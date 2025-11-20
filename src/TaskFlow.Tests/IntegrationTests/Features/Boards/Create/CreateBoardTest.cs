using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Create;

public class CreateBoardTest : TaskFlowClassFixture
{
    private const string Route = "Board";

    private readonly string _token;

    public CreateBoardTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = CreateBoardCommandBuilder.Build();

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
        var request = CreateBoardCommandBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(Route, request, _token);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}