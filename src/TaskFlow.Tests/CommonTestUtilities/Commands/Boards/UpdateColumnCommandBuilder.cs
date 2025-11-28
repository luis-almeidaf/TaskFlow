using TaskFlow.Application.Features.Boards.Commands.UpdateColumn;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public static class UpdateColumnCommandBuilder
{
    public static UpdateColumnCommand Build(Board board, Column column)
    {
        return new UpdateColumnCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            Name = "New Column Name"
        };
    }
}