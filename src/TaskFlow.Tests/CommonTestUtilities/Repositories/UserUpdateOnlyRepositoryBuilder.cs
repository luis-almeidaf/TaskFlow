using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Tests.CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateRepository Build(User user)
    {
        var mock = new Mock<IUserUpdateRepository>();

        mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return mock.Object;
    }
}