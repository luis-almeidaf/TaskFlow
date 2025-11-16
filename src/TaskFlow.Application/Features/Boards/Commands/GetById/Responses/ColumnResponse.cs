namespace TaskFlow.Application.Features.Boards.Commands.GetById.Responses;

public class ColumnResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}