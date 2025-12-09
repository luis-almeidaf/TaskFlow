using TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns;

public static class DeleteColumnCommandBuilder
{
    public static DeleteColumnCommand Build(Board board, Column column)
    {
        return new DeleteColumnCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id
        };
    }
}