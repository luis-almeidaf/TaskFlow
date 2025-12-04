using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Card;

namespace TaskFlow.Tests.Builders.Repositories;

public class CardReadOnlyRepositoryBuilder
{
    private readonly Mock<ICardReadOnlyRepository> _repository = new();

    public CardReadOnlyRepositoryBuilder GetCardById(
        User user, 
        Card card, 
        Guid boardId, 
        Guid columnId,
        Guid? cardId = null)
    {
        if (cardId.HasValue)
        {
            _repository.Setup(repo => repo.GetCardById(user, boardId, columnId, cardId.Value)).ReturnsAsync((Card?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetCardById(user, boardId, columnId, card.Id)).ReturnsAsync(card);
        }

        return this;
    }

    public ICardReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}