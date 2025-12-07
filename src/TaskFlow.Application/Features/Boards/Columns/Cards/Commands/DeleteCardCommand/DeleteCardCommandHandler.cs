using MediatR;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.DeleteCardCommand;

public class DeleteCardCommandHandler(
    ILoggedUser loggedUser,
    ICardWriteOnlyRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCardCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var user = await loggedUser.GetUserAndBoards();

        var card = await repository.GetById(user, request.BoardId, request.ColumnId, request.CardId);
        if (card is null) throw new CardNotFoundException();

        var deletedPosition = card.Position;

        repository.Delete(card);

        await repository.ReorderCards(request.ColumnId, deletedPosition);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}