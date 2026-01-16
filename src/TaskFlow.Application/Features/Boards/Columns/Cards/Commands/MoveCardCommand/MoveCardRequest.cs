namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;

public class MoveCardRequest
{
    public Guid? NewColumnId { get; set; }
    public int? NewPosition { get; set; }
}