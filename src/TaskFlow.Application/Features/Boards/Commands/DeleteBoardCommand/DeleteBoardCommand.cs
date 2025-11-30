using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}