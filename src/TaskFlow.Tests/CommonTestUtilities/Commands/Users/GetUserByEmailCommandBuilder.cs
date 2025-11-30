using TaskFlow.Application.Features.Users.Queries.GetByEmailQuery;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Users;

public static class GetUserByEmailCommandBuilder
{
    public static GetUserByEmailQuery Build(User user)
    {
        return new GetUserByEmailQuery
        {
            Email = user.Email
        };
    }
}