using Bogus;
using TaskFlow.Application.Features.Users.Commands.UpdateCommand;

namespace TaskFlow.Tests.Builders.Commands.Users;

public static class UpdateUserCommandBuilder
{
    public static UpdateUserCommand Build()
    {
        return new Faker<UpdateUserCommand>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}