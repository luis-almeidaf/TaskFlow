using Moq;
using TaskFlow.Domain.Repositories.RefreshToken;

namespace TaskFlow.Tests.Builders.Repositories;

public static class RefreshTokenWriteRepositoryBuilder
{
    public static IRefreshTokenWriteOnlyRepository Build()
    {
        var mock = new Mock<IRefreshTokenWriteOnlyRepository>();
        return mock.Object;
    }
}