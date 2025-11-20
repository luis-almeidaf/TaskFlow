using TaskFlow.Application.Features.Boards.Commands.GetById;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class GetBoardByIdCommandBuilder
{
    public static GetBoardByIdCommand Build(Board board)
    {
        return new GetBoardByIdCommand { Id = board.Id };
    }
}