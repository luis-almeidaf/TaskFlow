using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.AssignUserCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Cards.Commands.AssignUser;

public class AssignUserCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);
        board.Users.Add(user);

        var column = board.Columns.First();

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card);

        var request = AssignUserCommandBuilder.Build(user, board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First();

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card, boardId: board.Id);

        var request = AssignUserCommandBuilder.Build(user, board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_User_To_Be_Assigned_Not_In_Board()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First();

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card);
        
        var request = AssignUserCommandBuilder.Build(user,board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<UserNotInBoardException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_IN_BOARD));
    }

    [Fact]
    public async Task Error_Column_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First();

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card, columnId: column.Id);

        var request = AssignUserCommandBuilder.Build(user,board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = board.Columns.First();

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card, cardId: card.Id);

        var request = AssignUserCommandBuilder.Build(user, board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<CardNotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 & ex.GetErrors().Contains(ResourceErrorMessages.CARD_NOT_FOUND));
    }

    private static AssignUserCommandHandler CreateHandler(
        User user,
        Board board,
        Column column,
        Card card,
        Guid? boardId = null,
        Guid? columnId = null,
        Guid? cardId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.BuildUserWithBoards(user);
        var boardReadRepository = new BoardReadOnlyRepositoryBuilder();
        var cardRepository = new CardWriteOnlyRepositoryBuilder();
        var columnRepository = new ColumnReadOnlyRepositoryBuilder();

        boardReadRepository.GetById(user, board);
        cardRepository.GetById(user, board, column, card);
        columnRepository.GetById(column);

        if (boardId.HasValue)
            boardReadRepository.GetById(user, board, boardId);

        if (cardId.HasValue)
            cardRepository.GetById(user, board, column, card, cardId);

        if (columnId.HasValue)
            columnRepository.GetById(column, columnId);

        return new AssignUserCommandHandler(
            loggedUser,
            unitOfWork,
            boardReadRepository.Build(),
            cardRepository.Build(),
            columnRepository.Build());
    }
}