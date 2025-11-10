using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.Login;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands;

namespace TaskFlow.Tests.IntegrationTests.Features.Users.ChangePassword;

public class ChangePasswordTest : TaskFlowClassFixture
{
    private const string Route = "User/change-password";
    
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _password = webApplicationFactory.User.GetPassword();
        _email = webApplicationFactory.User.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = ChangePasswordCommandBuilder.Build();
        request.Password = _password;

        var response = await DoPut(Route, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new LoginCommand
        {
            Email = _email,
            Password = request.Password
        };

        response = await DoPost("Login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        loginRequest.Password =request.NewPassword;

        response = await DoPost("Login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Error_Password_Different_Current_Password()
    {
        var request = ChangePasswordCommandBuilder.Build();

        var response = await DoPut(Route, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}