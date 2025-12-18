using Mapster;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;

public class GetCardByIdQueryHandler(
    ICardReadOnlyRepository repository,
    IUserRetriever userRetriever)
    : IRequestHandler<GetCardByIdQuery, GetCardByIdResponse?>
{
    public async Task<GetCardByIdResponse?> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRetriever.GetCurrentUser();

        var card = await repository.GetById(user, request.BoardId, request.ColumnId, request.CardId);

        return card is null ? throw new CardNotFoundException() : card?.Adapt<GetCardByIdResponse>();
    }
}