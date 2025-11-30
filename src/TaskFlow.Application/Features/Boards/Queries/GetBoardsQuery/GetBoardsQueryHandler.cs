using Mapster;
using MediatR;
using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery.Responses;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;

namespace TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery;

public class GetBoardsQueryHandler : IRequestHandler<GetBoardsQuery, GetBoardsResponse>
{
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardReadOnlyRepository _repository;

    public GetBoardsQueryHandler(ILoggedUser loggedUser, IBoardReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<GetBoardsResponse> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(loggedUser);

        return new GetBoardsResponse()
        {
            Boards = result.Adapt<List<ShortBoardResponse>>()
        };
    }
}