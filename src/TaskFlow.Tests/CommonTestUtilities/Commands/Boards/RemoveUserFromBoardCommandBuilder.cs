using TaskFlow.Application.Features.Boards.Commands.DeleteUserFromBoard;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class RemoveUserFromBoardCommandBuilder
{
    public static DeleteUserFromBoardCommand Build(Board board, User user)
    {
        return new DeleteUserFromBoardCommand
        {
            BoardId = board.Id,
            UserId = user.Id
        };
    }
}