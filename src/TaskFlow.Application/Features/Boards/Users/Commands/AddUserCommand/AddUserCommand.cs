using MediatR;

namespace TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;

public class AddUserCommand : IRequest<AddUserResponse>
{
    public Guid BoardId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}