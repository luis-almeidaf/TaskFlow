using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class BoardMemberBuilder
{
    public static BoardMember Build(User user, Board board)
    {
        var boardMember = new Faker<BoardMember>()
            .RuleFor(boardMember => boardMember.Id, _ => Guid.NewGuid())
            .RuleFor(boardMember => boardMember.BoardId, _ => board.Id)
            .RuleFor(boardMember => boardMember.Board, _ => board)
            .RuleFor(boardMember => boardMember.UserId, _ => user.Id)
            .RuleFor(boardMember => boardMember.User, _ => user)
            .RuleFor(boardMember => boardMember.Role, faker => faker.PickRandom<BoardRole>());
        
        return boardMember;
    }
}