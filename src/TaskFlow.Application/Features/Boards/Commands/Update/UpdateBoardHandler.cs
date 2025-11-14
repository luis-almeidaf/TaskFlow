using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Commands.Update;

public class UpdateBoardHandler(
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser,
    IBoardWriteOnlyRepository repository)
    : IRequestHandler<UpdateBoardCommand, Unit>
{
    public async Task<Unit> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.Get();

        var board = await repository.GetById(user, request.Id);
        if (board is null) throw new BoardNotFoundException();
        
        board.Name = request.Name;

        repository.Update(board);

        await unitOfWork.Commit();

        return Unit.Value;
    }

    private static void Validate(UpdateBoardCommand request)
    {
        var result = new UpdateUserValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}