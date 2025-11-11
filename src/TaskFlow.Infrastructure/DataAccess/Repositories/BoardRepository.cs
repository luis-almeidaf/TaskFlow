using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class BoardRepository : IBoardWriteOnlyRepository, IBoardReadOnlyRepository
{
    private readonly TaskFlowDbContext _dbContext;

    public BoardRepository(TaskFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Board board)
    {
        await _dbContext.Boards.AddAsync(board);
    }

    public async Task<List<Board>> GetAll(User user)
    {
        return await _dbContext.Boards.AsNoTracking().Where(board => board.CreatedById == user.Id).ToListAsync();
    }

    public async Task<Board?> GetById(User user, Guid id)
    {
        return await _dbContext.Boards
            .AsNoTracking()
            .Include(board => board.CreatedBy)
            .Include(board => board.Users)
            .Include(board => board.Columns)
            .FirstOrDefaultAsync(board => board.Id == id && board.CreatedById == user.Id);
    }
}