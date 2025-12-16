using Mapster;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResponse?>
{
    private readonly IBoardReadOnlyRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetBoardByIdQueryHandler(IBoardReadOnlyRepository repository, ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<GetBoardByIdResponse?> Handle(GetBoardByIdQuery request,
        CancellationToken cancellationToken)
    {
        var loggedUser = await _currentUser.GetCurrentUser();

        var board = await _repository.GetById(loggedUser, request.Id);

        return board is null ? throw new BoardNotFoundException() : board?.Adapt<GetBoardByIdResponse>();
    }
}