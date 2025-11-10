using TaskFlow.Application.Features.Users.Commands.Delete;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class DeleteUserCommandBuilder
{
    public static DeleteUserCommand Build()
    {
        return new DeleteUserCommand();
    }
}