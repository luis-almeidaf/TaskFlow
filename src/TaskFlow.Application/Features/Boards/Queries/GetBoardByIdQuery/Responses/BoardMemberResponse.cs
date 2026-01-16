namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;

public class BoardMemberResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
}