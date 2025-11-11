using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.GetBoards;

public class GetBoardsCommand : IRequest<GetBoardsResponse> { }