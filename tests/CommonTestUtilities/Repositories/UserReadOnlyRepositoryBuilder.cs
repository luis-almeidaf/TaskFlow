using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(readOnlyRepository => readOnlyRepository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(readOnlyRepository => readOnlyRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);
        
        return this;
    }
    

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}