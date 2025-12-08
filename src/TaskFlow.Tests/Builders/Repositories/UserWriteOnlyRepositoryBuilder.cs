using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Tests.Builders.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository = new();

    public UserWriteOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }
    
    public IUserWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}