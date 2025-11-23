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

    public void Update(Board board)
    {
        _dbContext.Boards.Update(board);
    }

    public async Task Delete(Guid boardId)
    {
        var boardToRemove = await _dbContext.Boards.FindAsync(boardId);
        _dbContext.Boards.Remove(boardToRemove!);
    }

    async Task<Board?> IBoardWriteOnlyRepository.GetById(User user, Guid id)
    {
        return await _dbContext.Boards
            .Include(board => board.Users)
            .FirstOrDefaultAsync(board => board.Id == id && board.CreatedById == user.Id);
    }

    async Task<Board?> IBoardReadOnlyRepository.GetById(User user, Guid id)
    {
        return await _dbContext.Boards
            .AsNoTracking()
            .Include(board => board.CreatedBy)
            .Include(board => board.Users)
            .Include(board => board.Columns)
            .FirstOrDefaultAsync(board => board.Id == id && board.CreatedById == user.Id);
    }

    public void AddUserToBoard(Board board, User user)
    {
        _dbContext.Entry(user).State = EntityState.Unchanged;
        board.Users.Add(user);
    }

    public void RemoveUserFromBoard(Board board, User user)
    {
        board.Users.Remove(user);
    }

    public async Task AddColumnToBoard(Column column)
    {
        await _dbContext.Columns.AddAsync(column);
    }

    public async Task<Column?> GetColumnById(Guid id)
    {
        var column = await _dbContext.Columns.FirstOrDefaultAsync(user => user.Id == id);
        return column;
    }
}