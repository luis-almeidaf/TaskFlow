using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Queries.GetCardByIdQuery;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Cards.Queries.GetCardById;

public class GetCardByIdQueryHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, card, board.Id, column.Id);

        var request = new GetCardByIdQuery
        {
            BoardId = board.Id,
            CardId = card.Id,
            ColumnId = column.Id
        };

        var result = await handler.Handle(request, CancellationToken.None);

        result!.Id.Should().Be(request.CardId);
        result.Title.Should().Be(card.Title);
        result.Description.Should().Be(card.Description);
        result.CreatedAt.Should().NotBeAfter(DateTime.UtcNow);
        result.Position.Should().BeGreaterThanOrEqualTo(0);
        result.DueDate.Should().Be(card.DueDate);
        result.CreatedBy.Should().Be(card.CreatedBy);
        result.AssignedTo.Should().Be(card.AssignedTo);
    }

    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, card, board.Id, column.Id, cardId: card.Id);

        var request = new GetCardByIdQuery
        {
            BoardId = board.Id,
            CardId = card.Id,
            ColumnId = column.Id
        };

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<CardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CARD_NOT_FOUND));
    }

    private static GetCardByIdQueryHandler CreateHandler(
        User user,
        Card card,
        Guid boardId,
        Guid columnId,
        Guid? cardId = null)
    {
        var userRetriever = UserRetrieverBuilder.Build(user);
        var repository = new CardReadOnlyRepositoryBuilder();

        if (cardId.HasValue)
            repository.GetCardById(user, card, boardId, columnId, cardId);
        else
            repository.GetCardById(user, card, boardId, columnId);

        return new GetCardByIdQueryHandler(repository.Build(), userRetriever);
    }
}