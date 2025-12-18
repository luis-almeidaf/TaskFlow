using Mapster;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;

public class GetBoardByIdQueryHandler(IBoardReadOnlyRepository repository, IUserRetriever userRetriever)
    : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResponse?>
{
    public async Task<GetBoardByIdResponse?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        await userRetriever.GetCurrentUser();

        var board = await repository.GetById(request.Id);

        return board is null ? throw new BoardNotFoundException() : board?.Adapt<GetBoardByIdResponse>();
    }
}