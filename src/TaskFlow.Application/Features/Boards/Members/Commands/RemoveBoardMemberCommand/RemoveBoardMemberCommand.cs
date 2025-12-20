using MediatR;

namespace TaskFlow.Application.Features.Boards.Members.Commands.RemoveBoardMemberCommand;

public class RemoveBoardMemberCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid BoardMemberUserId { get; set; } 
}