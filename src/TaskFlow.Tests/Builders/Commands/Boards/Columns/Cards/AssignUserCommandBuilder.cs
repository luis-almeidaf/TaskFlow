using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.AssignUserCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;

public static class AssignUserCommandBuilder
{
    public static AssignUserCommand Build(User user, Board board, Column column, Card card)
    {
        return new AssignUserCommand
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            CardId = card.Id,
            AssignedToId = user.Id,
        };
    }
}