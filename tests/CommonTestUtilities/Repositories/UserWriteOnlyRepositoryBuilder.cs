using Moq;
using TaskFlow.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}