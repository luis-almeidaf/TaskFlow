using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    Task<Entities.Board?> GetById(Entities.User user, Guid id);
    void Update(Entities.Board board);
    Task Delete(Guid boardId);
    
    void AddUser(Entities.Board board, Entities.User user);
    void RemoveUser(Entities.Board board, Entities.User user);
}