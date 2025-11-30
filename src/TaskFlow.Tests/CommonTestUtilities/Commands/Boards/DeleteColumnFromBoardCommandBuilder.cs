using TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class DeleteColumnFromBoardCommandBuilder
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