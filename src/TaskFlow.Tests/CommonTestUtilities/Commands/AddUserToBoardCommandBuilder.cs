using Bogus;
using TaskFlow.Application.Features.Boards.Commands.AddUserToBoard;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.CommonTestUtilities.Commands;

public static class AddUserToBoardCommandBuilder
{
    public static AddUserToBoardCommand Build(Board board, User user)
    {
        return new AddUserToBoardCommand
        {
            BoardId = board.Id,
            UserEmail = user.Email
        };
    }
}