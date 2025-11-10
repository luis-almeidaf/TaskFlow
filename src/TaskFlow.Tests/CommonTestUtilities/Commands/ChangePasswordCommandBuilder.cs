using Bogus;
using TaskFlow.Application.Features.Users.Commands.ChangePassword;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public class ChangePasswordCommandBuilder
{
    public static ChangePasswordCommand Build()
    {
        return new Faker<ChangePasswordCommand>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}