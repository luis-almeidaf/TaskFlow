using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Commands.UpdateColumnCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.UpdateColumn;

public class UpdateColumnCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column);

        var request = UpdateColumnCommandBuilder.Build(board, column);

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

        var request = UpdateColumnCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var response = await act.Should().ThrowAsync<BoardNotFoundException>();
        
        response.Where(ex =>
            ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var user = UserBuilder.Build();
        var board = BoardBuilder.Build(user);
        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column, columnId: column.Id);

        var request = UpdateColumnCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var response = await act.Should().ThrowAsync<ColumnNotFoundException>();
        
        response.Where(ex =>
            ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }
    private static UpdateColumnCommandHandler CreateHandler(User user, Board board, Column column, Guid? boardId = null,
        Guid? columnId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var boardWriteRepository = new BoardWriteOnlyRepositoryBuilder();
        var boardReadRepository = new BoardReadOnlyRepositoryBuilder();

        if (boardId.HasValue)
            boardWriteRepository.GetById(user, board, boardId);
        else
            boardWriteRepository.GetById(user, board);

        if (columnId.HasValue)
            boardReadRepository.GetColumnById(column, columnId);
        else
            boardReadRepository.GetColumnById(column);

        return new UpdateColumnCommandHandler(unitOfWork, loggedUser, boardReadRepository.Build(),
            boardWriteRepository.Build());
    }
}