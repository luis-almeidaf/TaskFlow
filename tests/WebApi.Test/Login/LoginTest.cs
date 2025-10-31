using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using TaskFlow.Application.Features.Users.Commands.Login;
using TaskFlow.Exception;

namespace WebApi.Test.Login;

public class LoginTest : TaskFlowClassFixture
{
    private const string Route = "Login";

    private readonly string _name;
    private readonly string _email;
    private readonly string _password;

    public LoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _name = webApplicationFactory.GetName();
        _email = webApplicationFactory.GetEmail();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new LoginCommand()
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(Route, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Login_Invalid()
    {
        var request = LoginCommandBuilder.Build();

        var response = await DoPost(Route, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And
            .Contain(error => error.GetString()!.Equals(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }
    
}