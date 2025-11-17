using TaskFlow.Application.Features.Boards.Commands.Update;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class UpdateBoardCommandBuilder
{
    public static UpdateBoardCommand Build(Board board)
    {
        return new UpdateBoardCommand()
        {
            Id = board.Id,
            Name = "New Board name"
        };
    }
}