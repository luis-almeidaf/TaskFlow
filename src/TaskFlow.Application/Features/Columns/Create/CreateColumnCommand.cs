using MediatR;

namespace TaskFlow.Application.Features.Columns.Create
{
    public class CreateColumnCommand : IRequest<CreateColumnResponse>
    {
        public string Name { get; set; } = string.Empty;
        public Guid BoardId { get; set; }
    }
}