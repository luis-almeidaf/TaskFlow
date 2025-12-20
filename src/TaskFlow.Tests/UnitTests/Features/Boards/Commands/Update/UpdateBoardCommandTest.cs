using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.UpdateBoardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Commands.Update;

public class UpdateBoardCommandTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(board);

        var request = UpdateBoardCommandBuilder.Build(board);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var handler = CreateHandler(board, id: board.Id);

        var request = UpdateBoardCommandBuilder.Build(board);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    private static UpdateBoardCommandHandler CreateHandler(Board board, Guid? id = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();

        var repository = new BoardWriteOnlyRepositoryBuilder();
        if (id.HasValue)
        {
            repository.GetById(board, id);
        }
        else
        {
            repository.GetById(board);
        }

        return new UpdateBoardCommandHandler(unitOfWork, repository.Build());
    }
}