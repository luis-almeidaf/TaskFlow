using TaskFlow.Application.Features.Boards.Users.Commands.RemoveUserCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Users;

public class RemoveUserCommandBuilder
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