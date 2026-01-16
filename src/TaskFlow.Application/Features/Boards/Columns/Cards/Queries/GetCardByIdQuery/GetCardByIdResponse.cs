using TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery.Responses;

namespace TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;

public class GetCardByIdResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Position { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    public CreatorCardResponse CreatedBy { get; set; } = null!;
    public AssignedToResponse AssignedTo { get; set; } = null!;
}