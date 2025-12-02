namespace TaskFlow.Application.Features.Boards.Queries.GetBoardByIdQuery.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}