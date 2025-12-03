using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands;
using TaskFlow.Exception;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Columns.Cards.CreateCard;

public class CreateCardValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new CreateCardValidator();

        var user = UserBuilder.Build();
        
        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = CreateCardCommandBuilder.Build(board, column);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new CreateCardValidator();

        var user = UserBuilder.Build();
        
        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = CreateCardCommandBuilder.Build(board, column);
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.TITLE_CANNOT_BE_EMPTY));
    }
    
    [Fact]
    public void Error_Past_DueDate()
    {
        var validator = new CreateCardValidator();

        var user = UserBuilder.Build();
        
        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = CreateCardCommandBuilder.Build(board, column);
        request.DueDate = new DateTime(2023, 1, 15, 10, 0, 0);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.CARD_DUE_DATE_CANNOT_BE_FOR_THE_PAST));
    }
}