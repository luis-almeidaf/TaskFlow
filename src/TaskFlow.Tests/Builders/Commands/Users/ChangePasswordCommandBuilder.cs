using Bogus;
using TaskFlow.Application.Features.Users.Commands.ChangePasswordCommand;

namespace TaskFlow.Tests.Builders.Commands.Users;

public static class ChangePasswordCommandBuilder
{
    public static ChangePasswordCommand Build()
    {
        return new Faker<ChangePasswordCommand>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}