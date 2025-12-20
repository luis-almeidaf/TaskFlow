using Bogus;
using TaskFlow.Application.Features.Auth.Commands.Login;

namespace TaskFlow.Tests.Builders.Commands.Login;

public static class LoginCommandBuilder
{
    public static LoginCommand Build()
    {
        return new Faker<LoginCommand>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}