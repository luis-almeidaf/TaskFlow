using Bogus;
using TaskFlow.Application.Features.Users.Commands.Register;

namespace CommonTestUtilities.Requests;

public static class RegisterUserCommandBuilder
{
    public static RegisterUserCommand Build()
    {
        return new Faker<RegisterUserCommand>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}