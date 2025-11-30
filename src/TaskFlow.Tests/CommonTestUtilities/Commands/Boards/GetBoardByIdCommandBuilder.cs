using TaskFlow.Application.Features.Boards.Queries.GetByIdBoardQuery;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands.Boards;

public static class GetBoardByIdCommandBuilder
{
    public static GetBoardByIdQuery Build(Board board)
    {
        return new GetBoardByIdQuery { Id = board.Id };
    }
}