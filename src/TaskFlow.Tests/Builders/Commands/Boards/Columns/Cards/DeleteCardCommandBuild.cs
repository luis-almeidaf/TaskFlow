using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.DeleteCardCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;

public static class DeleteCardCommandBuild
{
    public static DeleteCardCommand Build(Board board, Column column, Card card)
    {
        return new DeleteCardCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            CardId = card.Id
        };
    }
}