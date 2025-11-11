using TaskFlow.Application.Features.Boards.Commands.GetBoards.Responses;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.Commands.GetBoards;

public class GetBoardsResponse
{
    public List<ShortBoardResponse> Boards { get; set; } = [];
}