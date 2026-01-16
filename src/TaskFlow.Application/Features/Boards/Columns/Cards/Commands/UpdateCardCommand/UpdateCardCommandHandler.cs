using Mapster;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;

public class UpdateCardCommandHandler(
    IUserRetriever userRetriever,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardRepository,
    ICardWriteOnlyRepository cardRepository,
    IColumnReadOnlyRepository columnRepository) : IRequestHandler<UpdateCardCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await userRetriever.GetCurrentUser();

        var board = await boardRepository.GetById(request.BoardId) ?? throw new BoardNotFoundException();

        var column = await columnRepository.GetById(board.Id, request.ColumnId) ?? throw new ColumnNotFoundException();

        var card = await cardRepository.GetById(user, board.Id, column.Id, request.CardId) ?? throw new CardNotFoundException();

        if (request.AssignedToId.HasValue)
        {
            var userInBoard = board.Members.Any(bm => bm.UserId == request.AssignedToId.Value);
            if (!userInBoard) throw new UserNotInBoardException();
        }

        request.Adapt(card);

        cardRepository.Update(card);

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