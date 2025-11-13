using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.RemoveUserFromBoard;

public class RemoveUserFromBoardHandler : IRequestHandler<RemoveUserFromBoardCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardReadOnlyRepository _readOnlyRepository;
    private readonly IBoardWriteOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public RemoveUserFromBoardHandler(IUnitOfWork unitOfWork, ILoggedUser loggedUser,
        IBoardReadOnlyRepository readOnlyRepository, IBoardWriteOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _readOnlyRepository = readOnlyRepository;
        _repository = repository;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<Unit> Handle(RemoveUserFromBoardCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.Get();

        var board = await _readOnlyRepository.GetByIdForUpdate(loggedUser, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var userToRemove = await _userReadOnlyRepository.GetById(request.UserId);
        if (userToRemove is null) throw new UserNotFoundException();

        var userExists = board.Users.Any(u => u.Id == userToRemove.Id);
        if (!userExists) throw new UserNotInBoardException();

        if (userToRemove.Id == board.CreatedById) throw new BoardOwnerCannotBeRemovedException();

        _repository.RemoveUserFromBoard(board, userToRemove);

        await _unitOfWork.Commit();

        return Unit.Value;
    }
}