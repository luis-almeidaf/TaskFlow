using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;

public class AddUserToBoardHandler : IRequestHandler<AddUserToBoardCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IBoardReadOnlyRepository _readOnlyRepository;
    private readonly IBoardWriteOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public AddUserToBoardHandler(
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser,
        IBoardReadOnlyRepository readOnlyRepository,
        IBoardWriteOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _readOnlyRepository = readOnlyRepository;
        _repository = repository;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<Unit> Handle(AddUserToBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        var loggedUser = await _loggedUser.Get();

        var board = await _readOnlyRepository.GetByIdForUpdate(loggedUser, request.BoardId);
        if (board is null) throw new BoardNotFoundException();
        
        var userToAdd = await _userReadOnlyRepository.GetUserByEmail(request.UserEmail);
        if (userToAdd is null) throw new UserNotFoundException();

        if (board.Users.Any(user => user.Id == userToAdd.Id)) throw new UserAlreadyInBoardException();
        
        _repository.AddUserToBoard(board , userToAdd);

        await _unitOfWork.Commit();

        return Unit.Value;
    }
    
    private void Validate(AddUserToBoardCommand request)
    {
        var result = new AddUserToBoardValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}