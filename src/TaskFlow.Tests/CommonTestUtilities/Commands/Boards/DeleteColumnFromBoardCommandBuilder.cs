using TaskFlow.Application.Features.Boards.Commands.DeleteColumnFromBoard;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class DeleteColumnFromBoardCommandBuilder
{
    public static DeleteColumnFromBoardCommand Build(Board board, Column column)
    {
        return new DeleteColumnFromBoardCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id
        };
    }
}