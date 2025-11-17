using TaskFlow.Application.Features.Boards.Commands.Delete;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public class DeleteBoardCommandBuilder
{
    public static DeleteBoardCommand Build(Board board)
    {
        return new DeleteBoardCommand { Id = board.Id };
    }
}