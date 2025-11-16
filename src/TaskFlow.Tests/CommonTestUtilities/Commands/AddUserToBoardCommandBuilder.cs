using Bogus;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class AddUserToBoardCommandBuilder
{
    public static AddUserToBoardCommand Build()
    {
        return new Faker<AddUserToBoardCommand>()
            .RuleFor(board => board.BoardId, Guid.NewGuid())
            .RuleFor(user => user.UserEmail, faker => faker.Internet.Email());
    }
}