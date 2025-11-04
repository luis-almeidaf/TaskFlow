using Moq;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Tests.CommonTestUtilities.Repositories;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}