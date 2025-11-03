using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.Create;

public class CreateBoardCommand : IRequest<CreateBoardResponse>
{
    public string Name { get; set; } = string.Empty;
}