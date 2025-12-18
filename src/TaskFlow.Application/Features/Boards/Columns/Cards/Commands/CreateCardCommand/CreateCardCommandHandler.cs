using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;

public class CreateCardCommandHandler(
    IUserRetriever userRetriever,
    IUnitOfWork unitOfWork,
    IBoardReadOnlyRepository boardReadOnlyRepository,
    ICardWriteOnlyRepository cardRepository,
    IColumnReadOnlyRepository columnRepository)
    : IRequestHandler<CreateCardCommand, CreateCardResponse>
{
    public async Task<CreateCardResponse> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await userRetriever.GetCurrentUser();

        var board = await boardReadOnlyRepository.GetById(request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await columnRepository.GetById(board.Id, request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        if (request.AssignedToId.HasValue)
        {
            var userInBoard = board.Members.Any(u => u.Id == request.AssignedToId.Value);
            if (!userInBoard) throw new UserNotInBoardException();
        }

        var card = request.Adapt<Card>();
        card.Id = Guid.NewGuid();
        card.CreatedById = user.Id;
        card.ColumnId = column.Id;
        card.AssignedToId = request.AssignedToId;

        var cardsCount = column.Cards.Count;
        card.Position = cardsCount;

        await cardRepository.Add(card);

        await unitOfWork.Commit();
        return new CreateCardResponse
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            CardId = card.Id,
            Title = card.Title,
            Description = card.Description,
            Position = card.Position
        };
    }

    private static void Validate(CreateCardCommand request)
    {
        var result = new CreateCardValidator().Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}