namespace TaskFlow.Application.Features.Boards.Queries.GetByIdBoardQuery.Responses;

public class ColumnResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}