using MediatR;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard.Requests;

namespace TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;

public class AddUserToBoardCommand : IRequest<AddUserToBoardResponse>
{
    public Guid BoardId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}