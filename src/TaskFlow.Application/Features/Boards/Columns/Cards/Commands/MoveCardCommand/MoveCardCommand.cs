using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;

public class MoveCardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid CurrentColumnId { get; set; }
    public Guid CardId { get; set; }
    public Guid? NewColumnId { get; set; }
    public int? NewPosition { get; set; }
}