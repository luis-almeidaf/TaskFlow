using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class BoardBuilder
{
    public static Board Build(User user)
    {
        var board = new Faker<Board>()
            .RuleFor(board => board.Id, _ => Guid.NewGuid())
            .RuleFor(board => board.Name, faker => faker.Commerce.Department())
            .RuleFor(board => board.CreatedById, _ => user.Id)
            .RuleFor(board => board.Members, _ => new List<BoardMember>())
            .RuleFor(board => board.Columns, _ => new List<Column>()).Generate();
        
        var columns = new List<Column>
        {
            new() { Id = Guid.NewGuid(), Name = "Todo", Position = 0, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "In Progress", Position = 1, BoardId = board.Id, Board = board },
            new() { Id = Guid.NewGuid(), Name = "Done", Position = 2, BoardId = board.Id, Board = board },
        };

        foreach (var column in columns)
            board.Columns.Add(column);
        
        return board;
    }
}