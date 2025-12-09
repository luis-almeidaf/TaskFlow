using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.AssignUserCommand;

public class AssignUserCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
    public Guid AssignedToId { get; set; }
}