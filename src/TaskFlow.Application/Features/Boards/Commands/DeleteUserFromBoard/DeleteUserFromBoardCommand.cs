using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteUserFromBoard;

public class DeleteUserFromBoardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; } 
}