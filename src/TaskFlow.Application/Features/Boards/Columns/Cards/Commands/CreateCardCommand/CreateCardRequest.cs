namespace TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;

public class CreateCardRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    
    public Guid? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
}