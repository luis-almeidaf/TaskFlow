using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.MoveCardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Cards.Commands.MoveCard;

public class MoveCardCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var firstColumn = board.Columns.First(c => c.Position == 1);
        var secondColumn = board.Columns.First(c => c.Position == 2);

        var card = CardBuilder.Build(firstColumn);
        firstColumn.Cards.Add(card);

        var handler = CreateHandler(user, board, firstColumn, secondColumn);

        var request = MoveCardCommandBuilder.Build(board, firstColumn, card, secondColumn.Id, 3);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Board_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var firstColumn = board.Columns.First(c => c.Position == 1);
        var secondColumn = board.Columns.First(c => c.Position == 2);

        var card = CardBuilder.Build(firstColumn);
        firstColumn.Cards.Add(card);

        var handler = CreateHandler(user, board, firstColumn, secondColumn, boardId: board.Id);

        var request = MoveCardCommandBuilder.Build(board, firstColumn, card, secondColumn.Id, 0);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<BoardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BOARD_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Card_Current_Column_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var firstColumn = board.Columns.First(c => c.Position == 1);
        var secondColumn = board.Columns.First(c => c.Position == 2);

        var card = CardBuilder.Build(firstColumn);
        firstColumn.Cards.Add(card);

        var handler = CreateHandler(user, board, firstColumn, secondColumn, firstColumnId: firstColumn.Id);

        var request = MoveCardCommandBuilder.Build(board, firstColumn, card, secondColumn.Id, 0);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Card_New_Column_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var firstColumn = board.Columns.First(c => c.Position == 1);
        var secondColumn = board.Columns.First(c => c.Position == 2);

        var card = CardBuilder.Build(firstColumn);
        firstColumn.Cards.Add(card);

        var handler = CreateHandler(user, board, firstColumn, secondColumn, secondColumnId: secondColumn.Id);

        var request = MoveCardCommandBuilder.Build(board, firstColumn, card, secondColumn.Id, 0);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ColumnNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.COLUMN_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var firstColumn = board.Columns.First(c => c.Position == 1);
        var secondColumn = board.Columns.First(c => c.Position == 2);

        var card = CardBuilder.Build(firstColumn);

        var handler = CreateHandler(user, board, firstColumn, secondColumn);

        var request = MoveCardCommandBuilder.Build(board, firstColumn, card, secondColumn.Id, 0);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<CardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CARD_NOT_FOUND));
    }

    private static MoveCardCommandHandler CreateHandler(
        User user,
        Board board,
        Column firstColumn,
        Column secondColumn,
        Guid? boardId = null,
        Guid? firstColumnId = null,
        Guid? secondColumnId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRetriever = UserRetrieverBuilder.Build(user);
        var boardRepository = new BoardReadOnlyRepositoryBuilder();
        var cardRepository = new CardWriteOnlyRepositoryBuilder();
        var columnRepository = new ColumnReadOnlyRepositoryBuilder();

        if (boardId.HasValue)
            boardRepository.GetById(user, board, boardId);
        else
            boardRepository.GetById(user, board);

        if (firstColumnId.HasValue)
            columnRepository.GetById(board.Id, firstColumn, firstColumnId);
        else if (secondColumnId.HasValue)
            columnRepository.GetById(board.Id, secondColumn, secondColumnId);
        else
        {
            columnRepository.GetById(board.Id, firstColumn);
            columnRepository.GetById(board.Id, secondColumn);
        }

        return new MoveCardCommandHandler(
            unitOfWork,
            userRetriever,
            boardRepository.Build(),
            cardRepository.Build(),
            columnRepository.Build());
    }
}