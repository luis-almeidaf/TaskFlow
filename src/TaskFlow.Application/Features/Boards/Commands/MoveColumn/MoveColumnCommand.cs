using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.MoveColumn;

public class MoveColumnCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public int NewPosition { get; set; }
}