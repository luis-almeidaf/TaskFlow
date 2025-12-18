using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Identity;

public interface IUserRetriever
{
    Task<User> GetCurrentUser();
}