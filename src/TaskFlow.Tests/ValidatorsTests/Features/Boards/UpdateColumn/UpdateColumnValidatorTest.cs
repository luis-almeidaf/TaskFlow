using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.CommonTestUtilities.Commands.Boards;
using TaskFlow.Tests.CommonTestUtilities.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.UpdateColumn;

public class UpdateColumnValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateColumnValidator();
        
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = UpdateColumnCommandBuilder.Build(board, column);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new UpdateColumnValidator();
        
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var request = UpdateColumnCommandBuilder.Build(board, column);
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().
            And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
}