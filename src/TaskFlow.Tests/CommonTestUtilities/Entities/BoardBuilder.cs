using Bogus;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Entities;

public static class BoardBuilder
{
    public static Board Build(User user)
    {
        var board = new Faker<Board>()
            .RuleFor(board => board.Id, _ => Guid.NewGuid())
            .RuleFor(board => board.Name, faker => faker.Commerce.Department())
            .RuleFor(board => board.CreatedById, _ => user.Id);

        return board;
    }
}