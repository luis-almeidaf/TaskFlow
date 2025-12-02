using MediatR;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;

public class GetBoardByIdQuery : IRequest<GetBoardByIdResponse?>
{
    public Guid Id { get; set; }
}