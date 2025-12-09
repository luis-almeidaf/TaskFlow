using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;

public static class MoveCardCommandBuilder
{
    public static MoveCardCommand Build(Board board, Column column, Card card, Guid newColumnId, int newPosition)
    {
        return new MoveCardCommand
        {
            BoardId = board.Id,
            CurrentColumnId = column.Id,
            CardId = card.Id,
            NewColumnId = newColumnId,
            NewPosition = newPosition
        };
    }
}