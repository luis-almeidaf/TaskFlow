namespace TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard;

public class CreateColumnResponse
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}