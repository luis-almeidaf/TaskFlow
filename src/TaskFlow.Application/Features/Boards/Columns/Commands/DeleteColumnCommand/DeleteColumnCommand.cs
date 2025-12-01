using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;

public class DeleteColumnCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
}
