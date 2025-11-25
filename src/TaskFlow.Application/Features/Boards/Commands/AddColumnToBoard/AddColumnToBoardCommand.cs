using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard
{
    public class AddColumnToBoardCommand : IRequest<AddColumnToBoardResponse>
    {
        public string Name { get; set; } = string.Empty;
        public Guid BoardId { get; set; }
    }
}