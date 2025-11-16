using TaskFlow.Application.Features.Boards.Commands.Update;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class UpdateBoardCommandBuilder
{
    public static UpdateBoardCommand Build()
    {
        return new UpdateBoardCommand()
        {
            Id = Guid.NewGuid(),
            Name = "New Board name"
        };
    }
}