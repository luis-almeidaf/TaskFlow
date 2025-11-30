using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.MoveColumn;

public class MoveColumnValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new MoveColumnValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = MoveColumnCommandBuilder.Build(board, column, 3);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-3)]
    [InlineData(-4)]
    public void Validate_Should_ReturnError_WhenNewPositionIsNegative(int position)
    {
        var validator = new MoveColumnValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = MoveColumnCommandBuilder.Build(board, column, 3);
        request.NewPosition = position;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.NEW_POSITION_CANNOT_BE_NEGATIVE));
    }
}