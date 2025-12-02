using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands;

public class CreateCardCommand : IRequest<CreateCardResponse>
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    public Guid? AssignedToId { get; set; }
    
    public DateTime? DueDate { get; set; }
}