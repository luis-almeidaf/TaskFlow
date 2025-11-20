using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Column;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class ColumnRepository : IColumnWriteOnlyRepository
{
    private readonly TaskFlowDbContext _dbContext;

    public ColumnRepository(TaskFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Column column)
    {
        await _dbContext.Columns.AddAsync(column);
    }
}
