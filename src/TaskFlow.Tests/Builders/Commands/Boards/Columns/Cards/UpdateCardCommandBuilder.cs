using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;

public static class UpdateCardCommandBuilder
{
    public static UpdateCardCommand Build(Board board, Column column, Card card)
    {
        return new UpdateCardCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            CardId = card.Id,
            Title = "New Title",
            Description = "New Description",
            AssignedToId = card.AssignedToId,
            DueDate = card.DueDate
        };
    }
}