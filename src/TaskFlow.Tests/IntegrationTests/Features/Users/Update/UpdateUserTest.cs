using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Users;

namespace TaskFlow.Tests.IntegrationTests.Features.Users.Update;

public class UpdateUserTest : TaskFlowClassFixture
{
    private const string Route = "User";

    private readonly string _token;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = UpdateUserCommandBuilder.Build();

        var response = await DoPut(Route, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = UpdateUserCommandBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPut(Route, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.NAME_EMPTY;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));

    }
}