using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.MoveColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Commands.MoveColumn;

public class MoveColumnCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First(c => c.Position == 1);

        var handler = CreateHandler(user, board);

        var request = MoveColumnCommandBuilder.Build(board, column, 2);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First(c => c.Position == 1);

        var handler = CreateHandler(user, board, boardId: board.Id);

        var request = MoveColumnCommandBuilder.Build(board, column, 2);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First(c => c.Position == 1);

        var handler = CreateHandler(user, board);

        var request = MoveColumnCommandBuilder.Build(board, column, 2);

        request.ColumnId = Guid.NewGuid();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    private static MoveColumnCommandHandler CreateHandler(User user, Board board, Guid? boardId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.BuildUserWithBoards(user);
        var repository = new BoardWriteOnlyRepositoryBuilder();

        if (boardId.HasValue)
            repository.GetById(user, board, boardId);
        else
            repository.GetById(user, board);

        return new MoveColumnCommandHandler(unitOfWork, loggedUser, repository.Build());
    }
}