using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IUserWriteOnlyRepository repository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(Commands.DeleteUserCommand.DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAndBoards();

        if (user.CreatedBoards.Any())
            throw new UserHasAssociatedBoardsException();

        await _repository.Delete(user);

        await _unitOfWork.Commit();

        return Unit.Value;
    }
}