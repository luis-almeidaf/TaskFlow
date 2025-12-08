using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.CreateCardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Cards.Commands.CreateCard;

public class CreateCardCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column);

        var request = CreateCardCommandBuilder.Build(board, column);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.BoardId.Should().Be(request.BoardId);
        result.ColumnId.Should().Be(request.ColumnId);
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Board_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column, boardId: board.Id);

        var request = CreateCardCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Column_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column, columnId: column.Id);

        var request = CreateCardCommandBuilder.Build(board, column);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    [Fact]
    public async Task Error_User_Not_In_Board()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var handler = CreateHandler(user, board, column);

        var request = CreateCardCommandBuilder.Build(board, column);

        request.AssignedToId = Guid.NewGuid();

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotInBoardException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_IN_BOARD));
    }

    private static CreateCardCommandHandler CreateHandler(User user, Board board, Column column, Guid? boardId = null,
        Guid? columnId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.BuildUserWithBoards(user);
        var boardReadRepository = new BoardReadOnlyRepositoryBuilder();
        var cardWriteRepository = new CardWriteOnlyRepositoryBuilder().Build();
        var columnReadRepository = new ColumnReadOnlyRepositoryBuilder();

        boardReadRepository.GetById(user, board);
        columnReadRepository.GetById(column);

        if (boardId.HasValue)
            boardReadRepository.GetById(user, board, boardId);

        if (columnId.HasValue)
            columnReadRepository.GetById(column, columnId);

        return new CreateCardCommandHandler(
            loggedUser,
            unitOfWork,
            boardReadRepository.Build(),
            cardWriteRepository,
            columnReadRepository.Build());
    }
}