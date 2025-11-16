using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.Update;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands;

namespace TaskFlow.Tests.ValidatorsTests.Boards.Update;

public class UpdateBoardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateBoardValidator();

        var request = UpdateBoardCommandBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    public void Error_Email_Empty(string name)
    {
        var validator = new UpdateBoardValidator();

        var request = UpdateBoardCommandBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}