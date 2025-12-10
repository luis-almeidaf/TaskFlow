using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(
    IUserWriteOnlyRepository repository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await loggedUser.GetUserAndBoards();

        if (user.CreatedBoards.Count != 0) throw new UserHasAssociatedBoardsException();

        await repository.Delete(user);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}