using MediatR;

namespace TaskFlow.Application.Features.Boards.Commands.GetAll;

public class GetBoardsCommand : IRequest<GetBoardsResponse> { }