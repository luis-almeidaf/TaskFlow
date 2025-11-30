using TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public static class MoveColumnCommandBuilder
{
    public static MoveColumnCommand Build(Board board, Column column, int newPosition)
    {
        return new MoveColumnCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            NewPosition = newPosition
        };
    }
}