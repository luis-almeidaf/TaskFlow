using FluentAssertions;
using FluentValidation;
using TaskFlow.Application.Common.Validators;
using TaskFlow.Application.Features.Users.Commands.Register;

namespace TaskFlow.Tests.ValidatorsTests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("             ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid_Register(string password)
    {
        var validator = new PasswordValidator<RegisterUserCommand>();

        var result = validator.IsValid(new ValidationContext<RegisterUserCommand>(new RegisterUserCommand()),
            password);

        result.Should().BeFalse();
    }
    
}