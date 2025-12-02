using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery.Responses;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardsQuery;

public class GetBoardsResponse
{
    public List<ShortBoardResponse> Boards { get; set; } = [];
}