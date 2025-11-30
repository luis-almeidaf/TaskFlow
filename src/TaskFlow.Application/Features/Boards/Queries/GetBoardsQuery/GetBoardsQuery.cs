using MediatR;

namespace TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery;

public class GetBoardsQuery : IRequest<GetBoardsResponse> { }