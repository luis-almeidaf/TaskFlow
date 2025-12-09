using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;

public static class CreateCardCommandBuilder
{
    public static CreateCardCommand Build(Board board, Column column)
    {
        return new CreateCardCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            Title = "New card",
        };
    }
}