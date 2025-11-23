using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteUserFromBoard;

public class DeleteUserFromBoardHandler(
    IUnitOfWork unitOfWork,
    ILoggedUser user,
    IBoardWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository)
    : IRequestHandler<DeleteUserFromBoardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserFromBoardCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await user.Get();

        var board = await repository.GetById(loggedUser, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var userToRemove = await userReadOnlyRepository.GetById(request.UserId);
        if (userToRemove is null) throw new UserNotFoundException();

        var userExists = board.Users.Any(u => u.Id == userToRemove.Id);
        if (!userExists) throw new UserNotInBoardException();

        if (userToRemove.Id == board.CreatedById) throw new BoardOwnerCannotBeRemovedException();

        repository.DeleteUserFromBoard(board, userToRemove);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}