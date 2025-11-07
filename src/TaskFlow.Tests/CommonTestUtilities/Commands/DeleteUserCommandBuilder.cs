using TaskFlow.Application.Features.Users.Commands.Delete;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public class DeleteUserCommandBuilder
{
    public static DeleteUserCommand Build()
    {
        return new DeleteUserCommand();
    }
}