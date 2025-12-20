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
    void UpdateBoardMember(BoardMember boardMember);
    Task<BoardMember?> GetBoardMember(Guid boardId, Guid boardMemberUserId);
    Task<Guid> GetOwnerId(Guid boardId);
}