using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Members.Commands.ChangeBoardMemberRoleCommand;

public class ChangeBoardMemberRoleRequest
{
    public BoardRole Role { get; set; }
}