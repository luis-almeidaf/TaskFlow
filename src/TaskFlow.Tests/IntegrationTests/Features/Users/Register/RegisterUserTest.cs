using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.RegisterUserCommand;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Users.Register;

public class RegisterUserTest : TaskFlowClassFixture
{
    private const string Route = "User";

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }

    [Fact]
    public async Task Success()
    {
        var request = new RegisterUserRequest
        {
            Email = "new_user@email.com",
            Name = "userRetriever",
            Password = "A!1qwerty"
        };

        var result = await DoPost(Route, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
        responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = new RegisterUserRequest
        {
            Email = "new_user2@email.com",
            Name = "",
            Password = "A!1qwerty"
        };

        var result = await DoPost(Route, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}