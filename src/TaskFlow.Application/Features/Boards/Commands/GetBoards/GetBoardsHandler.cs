using Mapster;
using MediatR;
using TaskFlow.Application.Features.Boards.Commands.GetBoards.Responses;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;

namespace TaskFlow.Application.Features.Boards.Commands.GetBoards;

public class GetBoardsHandler : IRequestHandler<GetBoardsCommand, GetBoardsResponse>
{
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardReadOnlyRepository _repository;

    public GetBoardsHandler(ILoggedUser loggedUser, IBoardReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<GetBoardsResponse> Handle(GetBoardsCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(loggedUser);

        return new GetBoardsResponse()
        {
            Boards = result.Adapt<List<ShortBoardResponse>>()
        };
    }
}