using TaskFlow.Application.Features.Boards.Commands.DeleteBoardCommand;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Builders.Commands.Boards;

public static class DeleteBoardCommandBuilder
{
    public static DeleteBoardCommand Build(Board board)
    {
        return new DeleteBoardCommand { Id = board.Id };
    }
}