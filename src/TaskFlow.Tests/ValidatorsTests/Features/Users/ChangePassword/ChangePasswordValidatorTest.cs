using FluentAssertions;
using TaskFlow.Application.Features.Users.Commands.ChangePassword;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Users;

namespace TaskFlow.Tests.ValidatorsTests.Features.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordCommandBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    public void Error_NewPassword_Empty(string newPassword)
    {
        var validator = new ChangePasswordValidator();

        var request = ChangePasswordCommandBuilder.Build();
        request.NewPassword = newPassword;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}