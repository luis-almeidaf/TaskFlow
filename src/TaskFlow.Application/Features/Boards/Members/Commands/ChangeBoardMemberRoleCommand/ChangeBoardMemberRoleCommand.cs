using MediatR;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Members.Commands.ChangeBoardMemberRoleCommand;

public class ChangeBoardMemberRoleCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid BoardMemberUserId { get; set; }
    public BoardRole Role { get; set; }
}