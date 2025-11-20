using TaskFlow.Application.Features.Users.Commands.GetByEmail;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Users;

public static class GetUserByEmailCommandBuilder
{
    public static GetUserByEmailCommand Build(User user)
    {
        return new GetUserByEmailCommand
        {
            Email = user.Email
        };
    }
}