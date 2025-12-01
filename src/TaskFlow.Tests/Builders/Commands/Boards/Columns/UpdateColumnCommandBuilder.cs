using TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns;

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