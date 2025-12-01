using MediatR;

namespace TaskFlow.Application.Features.Boards.Queries.GetByIdBoardQuery;

public class GetBoardByIdQuery : IRequest<GetBoardByIdResponse?>
{
    public Guid Id { get; set; }
}