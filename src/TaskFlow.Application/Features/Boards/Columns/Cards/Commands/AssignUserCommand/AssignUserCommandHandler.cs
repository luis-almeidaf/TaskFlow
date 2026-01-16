using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.AssignUserCommand;

public class AssignUserCommandHandler(
    IUserRetriever userRetriever,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardRepository,
    ICardWriteOnlyRepository cardRepository,
    IColumnReadOnlyRepository columnRepository) : IRequestHandler<AssignUserCommand, Unit>
{
    public async Task<Unit> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRetriever.GetCurrentUser();

        var board = await boardRepository.GetById(request.BoardId) ?? throw new BoardNotFoundException();

        var column = await columnRepository.GetById(board.Id, request.ColumnId) ?? throw new ColumnNotFoundException();

        var card = await cardRepository.GetById(user, board.Id, column.Id, request.CardId) ?? throw new CardNotFoundException();

        var userInBoard = board.Members.Any(bm => bm.UserId == request.AssignedToId);
        if (!userInBoard) throw new UserNotInBoardException();

        card.AssignedToId = request.AssignedToId;

        cardRepository.Update(card);

        await unitOfWork.Commit();

        return Unit.Value;
    }
}