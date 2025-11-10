using Bogus;
using TaskFlow.Application.Features.Login;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public class LoginCommandBuilder
{
    public static LoginCommand Build()
    {
        return new Faker<LoginCommand>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}