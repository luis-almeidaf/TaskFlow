using Bogus;
using TaskFlow.Application.Features.Users.Commands.Login;

namespace TaskFlow.Tests.CommonTestUtilities.Requests;

public class LoginCommandBuilder
{
    public static LoginCommand Build()
    {
        return new Faker<LoginCommand>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}