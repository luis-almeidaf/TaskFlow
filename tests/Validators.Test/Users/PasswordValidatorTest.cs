using FluentAssertions;
using FluentValidation;
using TaskFlow.Application.Common.Validators;
using TaskFlow.Application.UseCases;
using TaskFlow.Communication.Requests;

namespace Validators.Test.Users;

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
        var validator = new PasswordValidator<RequestRegisterUserDto>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserDto>(new RequestRegisterUserDto()),
            password);

        result.Should().BeFalse();
    }
    
}