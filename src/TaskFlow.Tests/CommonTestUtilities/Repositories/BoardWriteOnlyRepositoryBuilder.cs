using Moq;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Tests.CommonTestUtilities.Repositories;

public static class BoardWriteOnlyRepositoryBuilder
{
    public static IBoardWriteOnlyRepository Build()
    {
        var mock = new Mock<IBoardWriteOnlyRepository>();

        return mock.Object;
    }
}