using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.GetByID;

public class GetBoardByCommand : IRequest<GetBoardByIdResponse?>
{
    public Guid Id { get; set; }
}