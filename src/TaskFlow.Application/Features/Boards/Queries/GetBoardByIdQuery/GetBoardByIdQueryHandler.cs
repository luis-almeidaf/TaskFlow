using Mapster;
using MediatR;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;

public class GetBoardByIdQueryHandler(IBoardReadOnlyRepository repository) : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResponse?>
{
    public async Task<GetBoardByIdResponse?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await repository.GetById(request.Id);

        return board is null ? throw new BoardNotFoundException() : board?.Adapt<GetBoardByIdResponse>();
    }
}