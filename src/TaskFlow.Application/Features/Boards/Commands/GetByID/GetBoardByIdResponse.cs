using TaskFlow.Application.Features.Boards.Commands.GetByID.Responses;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Commands.GetByID;

public class GetBoardByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public CreatorBoardResponse CreatedBy { get; set; } = null!;
    public ICollection<UserResponse> Users { get; set; } = new HashSet<UserResponse>();
    public ICollection<ColumnResponse> Columns { get; set; } = new List<ColumnResponse>();
}

