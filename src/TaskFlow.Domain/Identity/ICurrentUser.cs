using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Identity;

public interface ICurrentUser
{
    Task<User> GetCurrentUser();
}