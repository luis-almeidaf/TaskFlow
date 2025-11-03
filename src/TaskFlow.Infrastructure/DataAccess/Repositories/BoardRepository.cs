using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class BoardRepository : IBoardWriteOnlyRepository
{
    private readonly TaskFlowDbContext _dbContext;

    public BoardRepository(TaskFlowDbContext dbContext) => _dbContext = dbContext;
    
    public async Task Add(Board board)
    {
        await _dbContext.Boards.AddAsync(board);
    }
}