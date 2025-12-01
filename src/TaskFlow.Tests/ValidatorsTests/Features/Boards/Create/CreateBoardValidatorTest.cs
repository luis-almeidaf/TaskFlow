using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.Builders.Commands.Boards;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Create;

public class CreateBoardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new CreateBoardValidator();

        var request = CreateBoardCommandBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new CreateBoardValidator();

        var request = CreateBoardCommandBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}