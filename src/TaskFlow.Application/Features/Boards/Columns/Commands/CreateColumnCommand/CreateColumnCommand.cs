using MediatR;

namespace TaskFlow.Application.Features.Boards.Columns.Commands.CreateColumnCommand
{
    public class CreateColumnCommand : IRequest<CreateColumnResponse>
    {
        public string Name { get; set; } = string.Empty;
        public Guid BoardId { get; set; }
    }
}