namespace TaskFlow.Application.Features.Boards.Commands.GetAll.Responses;

public class ShortBoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}