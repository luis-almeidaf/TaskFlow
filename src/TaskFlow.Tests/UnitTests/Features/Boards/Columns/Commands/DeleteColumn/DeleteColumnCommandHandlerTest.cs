using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.DeleteColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Commands.DeleteColumn;

public class DeleteColumnCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var columnToDelete = board.Columns.First(c => c.Position == 1);

        var handler = CreateHandler(user, board, columnToDelete);

        var request = DeleteColumnCommandBuilder.Build(board, columnToDelete);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column, boardId: board.Id);

        var request = DeleteColumnCommandBuilder.Build(board, column);

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

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column, columnId: column.Id);

        var request = DeleteColumnCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    private static DeleteColumnCommandHandler CreateHandler(
        User user,
        Board board,
        Column column,
        Guid? boardId = null,
        Guid? columnId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var boardWriteRepository = new BoardWriteOnlyRepositoryBuilder();

        if (boardId.HasValue)
            boardWriteRepository.GetById(user, board, boardId);
        else
            boardWriteRepository.GetById(user, board);

        var boardReadRepository = new BoardReadOnlyRepositoryBuilder();

        if (columnId.HasValue)
            boardReadRepository.GetColumnById(column, columnId);
        else
            boardReadRepository.GetColumnById(column);

        return new DeleteColumnCommandHandler(
            boardReadRepository.Build(),
            loggedUser,
            unitOfWork,
            boardWriteRepository.Build());
    }
}