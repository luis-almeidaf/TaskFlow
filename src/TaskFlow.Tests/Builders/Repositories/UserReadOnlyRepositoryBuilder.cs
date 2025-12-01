using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Tests.Builders.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(readOnlyRepository => readOnlyRepository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user, string? email = null)
    {
        _repository.Setup(readOnlyRepository => readOnlyRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        if (!string.IsNullOrWhiteSpace(email))
            _repository.Setup(readOnlyRepository => readOnlyRepository.GetUserByEmail(email)).ReturnsAsync((User?)null);

        return this;
    }

    public UserReadOnlyRepositoryBuilder GetById(User user, Guid? id = null)
    {
        _repository.Setup(readOnlyRepository => readOnlyRepository.GetById(user.Id)).ReturnsAsync(user);

        if (id.HasValue)
            _repository.Setup(readOnlyRepository => readOnlyRepository.GetById(id.Value))!.ReturnsAsync((User?)null);

        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}