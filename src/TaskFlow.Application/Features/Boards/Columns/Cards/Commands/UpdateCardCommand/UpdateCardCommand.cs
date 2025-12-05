using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.UpdateCardCommand;

public class UpdateCardCommand : IRequest<Unit>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public Guid? AssignedToId { get; set; }

    public DateTime? DueDate { get; set; }
}