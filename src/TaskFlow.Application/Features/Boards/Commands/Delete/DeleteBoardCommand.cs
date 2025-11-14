using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.Delete;

public class DeleteBoardCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}