using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.UpdateColumn;

public class UpdateColumnCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;
}