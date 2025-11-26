using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Entities;

public class ColumnBuilder
{
    public static Column Build(Board board)
    {
        var columnNames = new List<string> { "backlog", "waiting" };
        var columnPosition = new List<int> { 0, 1, 3, 4 };

        var column = new Faker<Column>()
            .RuleFor(column => column.Id, _ => Guid.NewGuid())
            .RuleFor(column => column.Name, faker => faker.PickRandom(columnNames))
            .RuleFor(column => column.Position, faker => faker.PickRandom(columnPosition))
            .RuleFor(column => column.BoardId, _ => board.Id);
        
        return column;
    }
}