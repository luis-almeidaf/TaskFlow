using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands;

namespace TaskFlow.Tests.ValidatorsTests.Boards.AddUserToBoard;

public class AddUserToBoardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new AddUserToBoardValidator();

        var request = AddUserToBoardCommandBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    public void Error_UserEmail_Empty(string email)
    {
        var validator = new AddUserToBoardValidator();

        var request = AddUserToBoardCommandBuilder.Build();
        request.UserEmail = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_UserEmail_Invalid()
    {
        var validator = new AddUserToBoardValidator();

        var request = AddUserToBoardCommandBuilder.Build();
        request.UserEmail = "invalid.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}