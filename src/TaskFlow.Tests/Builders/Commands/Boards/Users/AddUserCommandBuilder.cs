using TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Users;

public static class AddUserCommandBuilder
{
    public static AddBoardMemberCommand Build(Board board, User user)
    {
        return new AddBoardMemberCommand
        {
            BoardId = board.Id,
            UserEmail = user.Email
        };
    }
}