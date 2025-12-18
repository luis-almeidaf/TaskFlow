using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;

public class AddBoardMemberRequest()
{
    public string UserEmail { get; set; } = string.Empty;
    public BoardRole Role { get; set; }
} 