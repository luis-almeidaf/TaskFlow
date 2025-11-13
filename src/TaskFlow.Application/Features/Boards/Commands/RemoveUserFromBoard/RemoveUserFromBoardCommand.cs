using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.RemoveUserFromBoard;

public class RemoveUserFromBoardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; } 
}