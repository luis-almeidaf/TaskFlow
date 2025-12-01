using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;

public class UpdateBoardCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}