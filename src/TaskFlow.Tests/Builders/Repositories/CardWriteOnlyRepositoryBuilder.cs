using Moq;
using TaskFlow.Domain.Repositories.Card;

namespace TaskFlow.Tests.Builders.Repositories;

public class CardWriteOnlyRepositoryBuilder
{
    private readonly Mock<ICardWriteOnlyRepository> _repository = new();

    public ICardWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}