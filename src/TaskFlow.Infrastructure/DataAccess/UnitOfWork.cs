using TaskFlow.Domain.Repositories;

namespace TaskFlow.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly TaskFlowDbContext _dbContext;

    public UnitOfWork(TaskFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}