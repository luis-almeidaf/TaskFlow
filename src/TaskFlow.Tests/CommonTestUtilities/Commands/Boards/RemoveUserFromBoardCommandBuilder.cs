using TaskFlow.Application.Features.Boards.Commands.DeleteUserFromBoard;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class RemoveUserFromBoardCommandBuilder
{
    public static RemoveUserFromBoardCommand Build(Board board, User user)
    {
        return new RemoveUserFromBoardCommand
        {
            BoardId = board.Id,
            UserId = user.Id
        };
    }
}