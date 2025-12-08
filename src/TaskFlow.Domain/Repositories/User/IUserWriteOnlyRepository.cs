namespace TaskFlow.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task<Entities.User?> GetById(Guid id);
    Task Add(Entities.User user);
    Task Delete(Entities.User user);
    void Update(Entities.User user);
}