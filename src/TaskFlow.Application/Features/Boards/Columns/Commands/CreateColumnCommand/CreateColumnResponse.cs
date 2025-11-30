namespace TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand;

public class CreateColumnResponse
{
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}