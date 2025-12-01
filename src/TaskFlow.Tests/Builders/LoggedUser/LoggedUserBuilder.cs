using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Services.LoggedUser;

namespace TaskFlow.Tests.Builders.LoggedUser;

public static class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        return mock.Object;
    }
    
    public static ILoggedUser BuildUserWithBoards(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.GetUserAndBoards()).ReturnsAsync(user);

        return mock.Object;
    }
}