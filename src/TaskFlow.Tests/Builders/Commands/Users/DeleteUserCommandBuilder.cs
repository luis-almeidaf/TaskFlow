using TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;

namespace TaskFlow.Tests.Builders.Commands.Users;

public static class DeleteUserCommandBuilder
{
    public static DeleteUserCommand Build()
    {
        return new DeleteUserCommand();
    }
}