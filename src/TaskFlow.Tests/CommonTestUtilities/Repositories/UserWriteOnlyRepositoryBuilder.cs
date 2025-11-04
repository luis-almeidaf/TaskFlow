using Moq;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Tests.CommonTestUtilities.Repositories;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}