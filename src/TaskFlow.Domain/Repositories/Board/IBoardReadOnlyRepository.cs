using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardReadOnlyRepository
{
    Task<List<Entities.Board>> GetAll(Entities.User user);
    Task<Entities.Board?> GetById(Guid id);
    Task<BoardMember?> GetBoardMember(Guid boardId, Guid boardMemberId);
}