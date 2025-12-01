using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Columns.CreateColumn;

public class CreateColumnValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new CreateColumnValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);
        
        var request =AddColumnToBoardCommandBuilder.Build(board,column);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_Column_Name_Empty(string name)
    {
        var validator = new CreateColumnValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);
        column.Name = name;

        var request = AddColumnToBoardCommandBuilder.Build(board, column);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}
