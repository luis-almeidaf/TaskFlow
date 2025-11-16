using Mapster;
using MediatR;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;

namespace TaskFlow.Application.Features.Boards.Commands.GetById;

public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdCommand, GetBoardByIdResponse?>
{
    private readonly IBoardReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GetBoardByIdHandler(IBoardReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    
    public async Task<GetBoardByIdResponse?> Handle(GetBoardByIdCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        var board = await _repository.GetById(loggedUser, request.Id);

        return board?.Adapt<GetBoardByIdResponse>();
    }
}