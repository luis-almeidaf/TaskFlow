using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.IntegrationTests.Resources;

public class CardIdentityManager
{
    private readonly Card _card;

    public CardIdentityManager(Card card)
    {
        _card = card;
    }

    public Guid GetId() => _card.Id;
}