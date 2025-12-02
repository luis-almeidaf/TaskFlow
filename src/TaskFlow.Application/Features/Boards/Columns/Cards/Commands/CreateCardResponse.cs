using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands;

public class CreateCardResponse
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CardId { get; set; }
    public string Title { get; set; } = string.Empty;
}