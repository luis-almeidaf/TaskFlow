using Moq;
using TaskFlow.Domain.Repositories.Column;

namespace TaskFlow.Tests.Builders.Repositories;

public class ColumnWriteOnlyRepositoryBuilder
{
    private readonly Mock<IColumnWriteOnlyRepository> _mock = new();

    public IColumnWriteOnlyRepository Build()
    {
        return _mock.Object;
    }
}