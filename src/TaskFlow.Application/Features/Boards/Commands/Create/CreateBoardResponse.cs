namespace TaskFlow.Application.Features.Boards.Commands.Create;

public class CreateBoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}