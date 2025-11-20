using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.GetById;

public class GetBoardByIdCommand : IRequest<GetBoardByIdResponse?>
{
    public Guid Id { get; set; }
}