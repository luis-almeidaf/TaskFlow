using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
    Task<User> GetUserAndBoards();
}