using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class BoardMemberBuilder
{
    public static BoardMember Build(User user, Board board)
    {
        var boardMember = new BoardMember
        {
            Id = Guid.NewGuid(),
            Board = board,
            BoardId = board.Id,
            User = user,
            UserId = user.Id,
            Role = BoardRole.Guest
        };
        
        return boardMember;
    }
}