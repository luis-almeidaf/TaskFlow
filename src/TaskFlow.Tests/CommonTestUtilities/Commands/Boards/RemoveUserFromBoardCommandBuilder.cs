using TaskFlow.Application.Features.Boards.Users.Commands.RemoveUserCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class RemoveUserFromBoardCommandBuilder
{
    public static RemoveUserCommand Build(Board board, User user)
    {
        return new RemoveUserCommand
        {
            BoardId = board.Id,
            UserId = user.Id
        };
    }
}