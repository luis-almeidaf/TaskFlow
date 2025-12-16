using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Users.Commands.AddUserCommand;

public class AddUserCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IBoardWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository)
    : IRequestHandler<AddUserCommand, AddUserResponse>
{
    public async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        var loggedUser1 = await currentUser.GetCurrentUser();

        var board = await repository.GetById(loggedUser1, request.BoardId);
        if (board is null) throw new BoardNotFoundException();
        
        var userToAdd = await userReadOnlyRepository.GetUserByEmail(request.UserEmail);
        if (userToAdd is null) throw new UserNotFoundException();

        if (board.Users.Any(user => user.Id == userToAdd.Id)) throw new UserAlreadyInBoardException();
        
        repository.AddUser(board , userToAdd);

        await unitOfWork.Commit();

        return new AddUserResponse
        {
            Name = userToAdd.Name,
            Email = userToAdd.Email
        };
    }
    
    private static void Validate(AddUserCommand request)
    {
        var result = new AddUserValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}