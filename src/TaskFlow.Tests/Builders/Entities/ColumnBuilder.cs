using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class ColumnBuilder
{
    public static Column Build(Board board)
    {
        var columnNames = new List<string> { "backlog", "waiting" };


        var column = new Faker<Column>()
            .RuleFor(column => column.Id, _ => Guid.NewGuid())
            .RuleFor(column => column.Name, faker => faker.PickRandom(columnNames))
            .RuleFor(column => column.Position, _ => 3)
            .RuleFor(column => column.BoardId, _ => board.Id)
            .RuleFor(column => column.Board, _ => board);
        
        return column;
    }
}