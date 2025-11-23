using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteColumnFromBoard;

public class DeleteColumnFromBoardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
}
