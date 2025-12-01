using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Tests.Builders.Repositories;

public static class UserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateRepository Build(User user)
    {
        var mock = new Mock<IUserUpdateRepository>();

        mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return mock.Object;
    }
}