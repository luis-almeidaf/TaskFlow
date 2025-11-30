using Mapster;
using MediatR;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Queries.GetByIdBoardQuery;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResponse?>
{
    private readonly IBoardReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GetBoardByIdQueryHandler(IBoardReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    
    public async Task<GetBoardByIdResponse?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        var board = await _repository.GetById(loggedUser, request.Id);
        
        return board is null ? throw new BoardNotFoundException() : board?.Adapt<GetBoardByIdResponse>();
    }
}