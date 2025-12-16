using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;

namespace TaskFlow.Tests.Builders.LoggedUser;

public static class LoggedUserBuilder
{
    public static ICurrentUser Build(User user)
    {
        var mock = new Mock<ICurrentUser>();

        mock.Setup(loggedUser => loggedUser.GetCurrentUser()).ReturnsAsync(user);

        return mock.Object;
    }
}