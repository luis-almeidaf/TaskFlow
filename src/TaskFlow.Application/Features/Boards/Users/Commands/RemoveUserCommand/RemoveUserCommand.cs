using MediatR;

namespace TaskFlow.Application.Features.Boards.Users.Commands.RemoveUserCommand;

public class RemoveUserCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; } 
}