namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;

public class CardResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Position { get; init; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    public UserResponse CreatedBy { get; set; } = null!;
    public UserResponse? AssignedTo { get; set; }
}