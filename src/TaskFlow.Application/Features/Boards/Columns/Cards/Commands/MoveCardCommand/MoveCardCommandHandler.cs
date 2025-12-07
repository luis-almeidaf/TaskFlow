using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;

public class MoveCardCommandHandler(
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser,
    IBoardReadOnlyRepository boardRepository,
    ICardWriteOnlyRepository repository) : IRequestHandler<MoveCardCommand, Unit>
{
    public async Task<Unit> Handle(MoveCardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.GetUserAndBoards();

        var board = await boardRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await boardRepository.GetColumnById(request.CurrentColumnId);
        if (column is null) throw new ColumnNotFoundException();

        var cardsInCurrentColumn = column.Cards.OrderBy(card => card.Position).ToList();

        var cardToMove = cardsInCurrentColumn.FirstOrDefault(card => card.Id == request.CardId);
        if (cardToMove is null) throw new CardNotFoundException();

        if (request.NewColumnId.HasValue)
            await MoveCardBetweenColumns(request, cardsInCurrentColumn, cardToMove);
        else
            MoveCardInsideColumn(request, cardsInCurrentColumn, cardToMove);

        await unitOfWork.Commit();
        return Unit.Value;
    }

    private async Task MoveCardBetweenColumns(MoveCardCommand request, List<Card> cardsInCurrentColumn, Card cardToMove)
    {
        var newColumn = await boardRepository.GetColumnById(request.NewColumnId!.Value);
        if (newColumn is null) throw new ColumnNotFoundException();

        cardsInCurrentColumn.Remove(cardToMove);

        ReorderCardPositions(cardsInCurrentColumn);

        cardToMove.ColumnId = newColumn.Id;

        var targetCards = newColumn.Cards.OrderBy(card => card.Position).ToList();

        var targetPosition = request.NewPosition.HasValue
            ? Math.Min(request.NewPosition.Value, targetCards.Count)
            : targetCards.Count;

        targetCards.Insert(targetPosition, cardToMove);

        ReorderCardPositions(targetCards);
    }

    private void MoveCardInsideColumn(MoveCardCommand request, List<Card> cardsInCurrentColumn, Card cardToMove)
    {
        cardsInCurrentColumn.Remove(cardToMove);

        var targetPosition = Math.Min(request.NewPosition!.Value, cardsInCurrentColumn.Count);

        cardsInCurrentColumn.Insert(targetPosition, cardToMove);

        ReorderCardPositions(cardsInCurrentColumn);
    }

    private void ReorderCardPositions(List<Card> cards)
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].Position = i;
        }

        repository.UpdateRange(cards);
    }

    private static void Validate(MoveCardCommand request)
    {
        var result = new MoveCardValidator().Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}