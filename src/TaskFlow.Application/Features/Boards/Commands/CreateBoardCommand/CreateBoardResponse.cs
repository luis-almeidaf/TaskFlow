namespace TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}