using FluentAssertions;
using TaskFlow.Application.Features.Boards.Columns.Cards.Commands.DeleteCardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards.Columns.Cards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.Repositories;
using TaskFlow.Tests.Builders.UserRetriever;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Columns.Cards.Commands.DeleteCard;

public class DeleteCardCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card);

        var request = DeleteCardCommandBuilder.Build(board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var user = UserBuilder.Build();

        var board = BoardBuilder.Build(user);

        var column = ColumnBuilder.Build(board);

        var card = CardBuilder.Build(column);

        var handler = CreateHandler(user, board, column, card, cardId: card.Id);

        var request = DeleteCardCommandBuilder.Build(board, column, card);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<CardNotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CARD_NOT_FOUND));
    }

    private static DeleteCardCommandHandler CreateHandler(
        User user,
        Board board,
        Column column,
        Card card,
        Guid? cardId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRetriever = UserRetrieverBuilder.Build(user);
        var cardRepository = new CardWriteOnlyRepositoryBuilder();

        cardRepository.GetById(user, board, column, card);

        if (cardId.HasValue)
            cardRepository.GetById(user, board, column, card, cardId);

        return new DeleteCardCommandHandler(
            userRetriever,
            cardRepository.Build(),
            unitOfWork);
    }
}