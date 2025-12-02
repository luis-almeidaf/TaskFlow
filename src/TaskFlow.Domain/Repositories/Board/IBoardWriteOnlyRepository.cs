using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    Task<Entities.Board?> GetById(Entities.User user, Guid id);
    void Update(Entities.Board board);
    Task Delete(Guid boardId);
    
    void AddUserToBoard(Entities.Board board, Entities.User user);
    void DeleteUserFromBoard(Entities.Board board, Entities.User user);

    Task AddColumnToBoard(Column column);
    void DeleteColumnFromBoard(Column column);
    void ReorderColumns(Entities.Board board, int position);
    void UpdateColumn(Column column);

    Task AddCardToColumn(Card card);
}