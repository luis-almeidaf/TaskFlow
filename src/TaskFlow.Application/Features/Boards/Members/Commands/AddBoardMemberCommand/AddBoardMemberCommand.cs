using MediatR;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;

public class AddBoardMemberCommand : IRequest<AddBoardMemberResponse>
{
    public Guid BoardId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public BoardRole Role { get; set; }
}