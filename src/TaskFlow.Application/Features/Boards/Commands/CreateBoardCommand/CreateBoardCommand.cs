using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardCommand : IRequest<CreateBoardResponse>
{
    public string Name { get; set; } = string.Empty;
}