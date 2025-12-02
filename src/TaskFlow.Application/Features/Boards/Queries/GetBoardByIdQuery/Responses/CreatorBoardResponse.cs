namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;

public class CreatorBoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}