using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    Task<Entities.Board?> GetById(Guid id);
    void Update(Entities.Board board);
    Task Delete(Guid boardId);
    
    void AddBoardMember(BoardMember boardMember);
    void RemoveBoardMember(BoardMember boardMember);
    Task<BoardMember?> GetBoardMember(Guid boardId, Guid boardMemberId);
    Task<Guid> GetOwnerId(Guid boardId);
}