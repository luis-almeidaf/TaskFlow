using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Columns.Cards.MoveCard;

public class MoveCardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new MoveCardValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var fistColumn = board.Columns.First(c => c.Position == 0);
        var secondColumn = board.Columns.First(c => c.Position == 1);

        var card = CardBuilder.Build(fistColumn);

        var request = MoveCardCommandBuilder.Build(board, fistColumn, card, secondColumn.Id, 1);

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
        var validator = new MoveCardValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var fistColumn = board.Columns.First(c => c.Position == 0);
        var secondColumn = board.Columns.First(c => c.Position == 1);

        var card = CardBuilder.Build(fistColumn);

        var request = MoveCardCommandBuilder.Build(board, fistColumn, card, secondColumn.Id, position);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceErrorMessages.NEW_POSITION_CANNOT_BE_NEGATIVE));
    }
}