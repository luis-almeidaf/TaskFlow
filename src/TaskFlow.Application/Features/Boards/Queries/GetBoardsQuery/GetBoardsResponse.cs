using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery.Responses;

namespace TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery;

public class GetBoardsResponse
{
    public List<ShortBoardResponse> Boards { get; set; } = [];
}