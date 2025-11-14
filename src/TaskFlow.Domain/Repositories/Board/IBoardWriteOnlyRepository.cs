namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    void AddUserToBoard(Entities.Board board, Entities.User user);
    void RemoveUserFromBoard(Entities.Board board, Entities.User user);
    Task<Entities.Board?> GetById(Entities.User user, Guid id);
    void Update(Entities.Board board);
    Task Delete(Guid boardId);
}