using Mapster;
using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands;

public class CreateCardCommandHandler(
    IBoardReadOnlyRepository boardReadOnlyRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork,
    IBoardWriteOnlyRepository repository)
    : IRequestHandler<CreateCardCommand, CreateCardResponse>
{
    public async Task<CreateCardResponse> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var user = await loggedUser.GetUserAndBoards();

        var board = await boardReadOnlyRepository.GetById(user, request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var column = await boardReadOnlyRepository.GetColumnById(request.ColumnId);
        if (column is null) throw new ColumnNotFoundException();

        if (request.AssignedToId.HasValue)
        {
            var userInBoard = board.Users.Any(u => u.Id == request.AssignedToId.Value);
            if (!userInBoard) throw new UserNotInBoardException();
        }

        var card = request.Adapt<Card>();
        card.CreatedById = user.Id;
        card.Id = Guid.NewGuid();
        card.ColumnId = column.Id;
        card.AssignedToId = request.AssignedToId;

        await repository.AddCardToColumn(card);

        await unitOfWork.Commit();
        return new CreateCardResponse
        {
            BoardId = board.Id,
            ColumnId = column.Id,
            CardId = card.Id,
            Title = card.Title,
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