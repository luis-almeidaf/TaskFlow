namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    Task<Entities.Board?> GetById(Entities.User user, Guid id);
    void Update(Entities.Board board);
    Task Delete(Guid boardId);
    
    void AddUserToBoard(Entities.Board board, Entities.User user);
    void RemoveUserFromBoard(Entities.Board board, Entities.User user);

    Task AddColumnToBoard(Entities.Column column);
    Task<Entities.Column?> GetColumnById(Guid id);
}