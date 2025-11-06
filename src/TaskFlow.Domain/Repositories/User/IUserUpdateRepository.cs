namespace TaskFlow.Domain.Repositories.User;

public interface IUserUpdateRepository
{
    Task<Entities.User> GetById(Guid id);
    void Update(Entities.User user);
}