namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;

public class ColumnResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public ICollection<CardResponse> Cards { get; set; } = new List<CardResponse>();
}