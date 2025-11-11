namespace TaskFlow.Application.Features.Boards.Commands.GetByID.Responses;

public class ColumnResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}