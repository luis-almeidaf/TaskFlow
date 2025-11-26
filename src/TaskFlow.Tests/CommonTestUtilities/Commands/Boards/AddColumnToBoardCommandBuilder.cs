using TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public class AddColumnToBoardCommandBuilder
{
    public static AddColumnToBoardCommand Build(Board board, Column column)
    {
        return new AddColumnToBoardCommand
        {
            BoardId = board.Id,
            Name = column.Name
        };
    }
}