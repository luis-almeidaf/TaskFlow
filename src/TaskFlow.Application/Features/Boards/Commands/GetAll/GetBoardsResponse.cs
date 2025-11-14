using TaskFlow.Application.Features.Boards.Commands.GetAll.Responses;

namespace TaskFlow.Application.Features.Boards.Commands.GetAll;

public class GetBoardsResponse
{
    public List<ShortBoardResponse> Boards { get; set; } = [];
}