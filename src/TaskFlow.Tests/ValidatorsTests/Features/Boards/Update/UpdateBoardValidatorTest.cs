using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Update;

public class UpdateBoardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateBoardValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var request = UpdateBoardCommandBuilder.Build(board);

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
        
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);
        
        var request = UpdateBoardCommandBuilder.Build(board);
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}