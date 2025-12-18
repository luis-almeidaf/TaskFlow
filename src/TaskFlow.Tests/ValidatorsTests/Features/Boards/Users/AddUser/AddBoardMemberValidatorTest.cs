using FluentAssertions;
using TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;
using TaskFlow.Exception;
using TaskFlow.Tests.Builders.Commands.Boards.Users;
using TaskFlow.Tests.Builders.Entities;

namespace TaskFlow.Tests.ValidatorsTests.Features.Boards.Users.AddUser;

public class AddBoardMemberValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new AddBoardMemberValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var request = AddUserCommandBuilder.Build(board, user);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    public void Error_UserEmail_Empty(string email)
    {
        var validator = new AddBoardMemberValidator();

        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var request = AddUserCommandBuilder.Build(board, user);
        request.UserEmail = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_UserEmail_Invalid()
    {
        var validator = new AddBoardMemberValidator();

        var user = UserBuilder.Build();
        
        var board = BoardBuilder.Build(user);

        var request = AddUserCommandBuilder.Build(board, user);
        request.UserEmail = "invalid.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}