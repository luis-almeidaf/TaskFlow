using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(
    IUserWriteOnlyRepository userRepository,
    IBoardReadOnlyRepository boardRepository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await currentUser.GetCurrentUser();
        var boards = await boardRepository.GetAll(user);
        
        if (boards.Count != 0) throw new UserHasAssociatedBoardsException();

        await userRepository.Delete(user);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}