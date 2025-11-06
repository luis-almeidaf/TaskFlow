using MediatR;

namespace TaskFlow.Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}