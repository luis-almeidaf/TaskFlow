using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;

public class AddUserToBoardHandler(
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser,
    IBoardWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository)
    : IRequestHandler<AddUserToBoardCommand, Unit>
{
    public async Task<Unit> Handle(AddUserToBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        var loggedUser1 = await loggedUser.Get();

        var board = await repository.GetById(loggedUser1, request.BoardId);
        if (board is null) throw new BoardNotFoundException();
        
        var userToAdd = await userReadOnlyRepository.GetUserByEmail(request.UserEmail);
        if (userToAdd is null) throw new UserNotFoundException();

        if (board.Users.Any(user => user.Id == userToAdd.Id)) throw new UserAlreadyInBoardException();
        
        repository.AddUserToBoard(board , userToAdd);

        await unitOfWork.Commit();

        return Unit.Value;
    }
    
    private static void Validate(AddUserToBoardCommand request)
    {
        var result = new AddUserToBoardValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}