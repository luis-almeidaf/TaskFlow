using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using TaskFlow.Exception;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : TaskFlowClassFixture
{
    private const string Route = "User";
    
    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }

    [Fact]
    public async Task Success()
    {
        var request = RegisterUserCommandBuilder.Build();

        var result = await DoPost(Route, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RegisterUserCommandBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(Route, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}