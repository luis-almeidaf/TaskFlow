using TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns;

public static class CreateColumnCommandBuilder
{
    public static CreateColumnCommand Build(Board board, Column column)
    {
        return new CreateColumnCommand
        {
            BoardId = board.Id,
            Name = column.Name
        };
    }
}