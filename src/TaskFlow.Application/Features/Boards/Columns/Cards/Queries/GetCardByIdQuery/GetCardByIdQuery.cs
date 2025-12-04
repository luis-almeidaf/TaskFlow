using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;

public class GetCardByIdQuery : IRequest<GetCardByIdResponse?>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
}