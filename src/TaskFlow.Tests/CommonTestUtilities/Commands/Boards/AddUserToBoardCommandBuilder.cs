using TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public static class AddUserToBoardCommandBuilder
{
    public static AddUserCommand Build(Board board, User user)
    {
        return new AddUserCommand
        {
            BoardId = board.Id,
            UserEmail = user.Email
        };
    }
}