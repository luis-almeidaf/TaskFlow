using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Card;

namespace TaskFlow.Tests.Builders.Repositories;

public class CardWriteOnlyRepositoryBuilder
{
    private readonly Mock<ICardWriteOnlyRepository> _repository = new();

    public CardWriteOnlyRepositoryBuilder GetById(User user, Board board, Column column, Card card, Guid? cardId = null)
    {
        if (cardId.HasValue)
        {
            _repository.Setup(repo => repo.GetById(It.IsAny<User>(), board.Id, column.Id, cardId.Value)).ReturnsAsync((Card?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetById(It.IsAny<User>(), board.Id, column.Id, card.Id)).ReturnsAsync(card);
        }

        return this;
    }

    public ICardWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}