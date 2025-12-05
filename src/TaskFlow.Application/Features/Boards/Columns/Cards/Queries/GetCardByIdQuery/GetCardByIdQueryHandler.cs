using Mapster;
using MediatR;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;

public class GetCardByIdQueryHandler(
    ICardReadOnlyRepository repository,
    ILoggedUser loggedUser)
    : IRequestHandler<GetCardByIdQuery, GetCardByIdResponse?>
{
    public async Task<GetCardByIdResponse?> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await loggedUser.Get();

        var card = await repository.GetById(user, request.BoardId, request.ColumnId, request.CardId);

        return card is null ? throw new CardNotFoundException() : card?.Adapt<GetCardByIdResponse>();
    }
}