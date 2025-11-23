using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.AddColumnToBoard
{
    public class CreateColumnCommand : IRequest<CreateColumnResponse>
    {
        public string Name { get; set; } = string.Empty;
        public Guid BoardId { get; set; }
    }
}