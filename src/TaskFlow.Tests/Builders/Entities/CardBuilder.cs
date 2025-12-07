using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Entities;

public static class CardBuilder
{
    public static Card Build(Column column)
    {
        var card = new Faker<Card>()
            .RuleFor(card => card.Id, _ => Guid.NewGuid())
            .RuleFor(card => card.Title, faker => faker.Lorem.Word())
            .RuleFor(card => card.Description, faker => faker.Lorem.Sentence(5))
            .RuleFor(card => card.DueDate, faker => faker.Date.Future())
            .RuleFor(card => card.CreatedById, _ => column.Board.CreatedById)
            .RuleFor(card => card.AssignedToId, _ => column.Board.CreatedById)
            .RuleFor(card => card.ColumnId, _ => column.Id);

        column.Cards.Add(card);

        return card;
    }
}