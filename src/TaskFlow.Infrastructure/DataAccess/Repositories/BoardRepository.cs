using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class BoardRepository(TaskFlowDbContext dbContext) : IBoardWriteOnlyRepository, IBoardReadOnlyRepository
{
    public async Task<List<Board>> GetAll(User user)
    {
        return await dbContext.Boards
            .AsNoTracking()
            .Where(board => board.CreatedById == user.Id || board.Members.Any(u => u.Id == user.Id)).ToListAsync();
    }

    async Task<Board?> IBoardWriteOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Boards
            .Include(board => board.Members)
            .Include(board => board.Columns)
            .FirstOrDefaultAsync(board => board.Id == id);
    }

    public async Task<BoardRole?> GetUserRoleInBoard(BoardMember boardMember)
    {
        var member = await dbContext.BoardMembers.FirstOrDefaultAsync((bm =>
            bm.UserId == boardMember.UserId && bm.BoardId == boardMember.BoardId));
        return member?.Role;
    }

    async Task<Board?> IBoardReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Boards
            .AsNoTracking()
            .Include(board => board.CreatedBy)
            .Include(board => board.Members)
            .Include(board => board.Columns)
            .ThenInclude(column => column.Cards)
            .ThenInclude(card => card.CreatedBy)
            .Include(board => board.Columns)
            .ThenInclude(column => column.Cards)
            .ThenInclude(card => card.AssignedTo)
            .FirstOrDefaultAsync(board => board.Id == id);
    }

    public async Task Add(Board board)
    {
        await dbContext.Boards.AddAsync(board);
    }

    public void Update(Board board)
    {
        dbContext.Boards.Update(board);
    }

    public async Task Delete(Guid boardId)
    {
        var boardToRemove = await dbContext.Boards.FindAsync(boardId);
        dbContext.Boards.Remove(boardToRemove!);
    }

    public void AddBoardMember(BoardMember boardMember)
    {
        dbContext.BoardMembers.Add(boardMember);
    }

    public void RemoveBoardMember(BoardMember boardMember)
    {
        dbContext.BoardMembers.Remove(boardMember);
    }

    public async Task<BoardMember?> GetBoardMember(Guid boardId, Guid boardMemberId)
    {
        return await dbContext.BoardMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(bm => bm.BoardId == boardId && bm.UserId == boardMemberId);
    }

    public async Task<Guid> GetOwnerId(Guid boardId)
    {
        var boardOwner = await dbContext.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
        return boardOwner!.CreatedById;
    }
}