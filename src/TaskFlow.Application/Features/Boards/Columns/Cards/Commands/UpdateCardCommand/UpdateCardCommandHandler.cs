using Mapster;
using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;

public class UpdateCardCommandHandler(
    IBoardReadOnlyRepository boardReadOnlyRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork,
    ICardWriteOnlyRepository repository) : IRequestHandler<UpdateCardCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.Get();

        var board = await boardReadOnlyRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await boardReadOnlyRepository.GetColumnById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        var card = await repository.GetById(user, board.Id, column.Id, request.CardId);
        if (card is null) throw new CardNotFoundException();
        
        if (request.AssignedToId.HasValue)
        {
            var userInBoard = board.Users.Any(u => u.Id == request.AssignedToId.Value);
            if (!userInBoard) throw new UserNotInBoardException();
        }

        request.Adapt(card);

        repository.Update(card);

        await unitOfWork.Commit();
        
        return Unit.Value;
    }
    
    private static void Validate(UpdateCardCommand request)
    {
        var result = new UpdateCardValidator().Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}