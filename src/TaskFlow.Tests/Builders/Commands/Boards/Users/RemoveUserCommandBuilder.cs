using TaskFlow.Application.Features.Boards.Members.Commands.RemoveBoardMemberCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Users;

public static class RemoveUserCommandBuilder
{
    public static RemoveBoardMemberCommand Build(Board board, BoardMember boardMember)
    {
        return new RemoveBoardMemberCommand
        {
            BoardId = board.Id,
            BoardMemberId = boardMember.Id
        };
    }
}