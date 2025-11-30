using TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public static class CreateBoardCommandBuilder
{
    public static CreateBoardCommand Build()
    {
        return new CreateBoardCommand()
        {
            Name = "New Board"
        };
    }
}