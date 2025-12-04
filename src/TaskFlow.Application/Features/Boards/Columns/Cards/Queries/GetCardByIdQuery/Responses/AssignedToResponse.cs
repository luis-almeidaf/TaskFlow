namespace TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery.Responses;

public class AssignedToResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}