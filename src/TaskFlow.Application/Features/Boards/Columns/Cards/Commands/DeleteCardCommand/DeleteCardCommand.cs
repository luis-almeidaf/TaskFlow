using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.DeleteCardCommand;

public class DeleteCardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
}