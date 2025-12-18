using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;

namespace TaskFlow.Tests.Builders.UserRetriever;

public static class UserRetrieverBuilder
{
    public static IUserRetriever Build(User user)
    {
        var mock = new Mock<IUserRetriever>();

        mock.Setup(userRetriever => userRetriever.GetCurrentUser()).ReturnsAsync(user);

        return mock.Object;
    }
}