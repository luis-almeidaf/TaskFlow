using Mapster;
using MediatR;
using TaskFlow.Application.Features.Boards.Queries.GetAllBoardsQuery.Responses;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardsQuery;

public class GetBoardsQueryHandler(
    ICurrentUser currentUser, 
    IBoardReadOnlyRepository boardRepository) : IRequestHandler<GetBoardsQuery, GetBoardsResponse>
{
    public async Task<GetBoardsResponse> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        var loggedUser = await currentUser.GetCurrentUser();

        var result = await boardRepository.GetAll(loggedUser);

        return new GetBoardsResponse()
        {
            Boards = result.Adapt<List<ShortBoardResponse>>()
        };
    }
}