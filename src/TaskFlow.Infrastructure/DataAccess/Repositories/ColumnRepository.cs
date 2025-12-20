using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Column;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class ColumnRepository(TaskFlowDbContext dbContext) : IColumnReadOnlyRepository, IColumnWriteOnlyRepository
{
    public async Task Add(Column column)
    {
        await dbContext.Columns.AddAsync(column);
    }

    public void Delete(Column column)
    {
        dbContext.Columns.Remove(column);
    }

    public void ReorderColumns(Board board, int position)
    {
        var columns = board.Columns.Where(c => c.Position > position).ToList();

        foreach (var column in columns)
        {
            column.Position--;
        }
    }

    public void Update(Column column)
    {
        dbContext.Columns.Update(column);
    }

    public void UpdateRange(IEnumerable<Column> columns)
    {
        dbContext.Columns.UpdateRange(columns);
    }
    
    public async Task<Column?> GetById(Guid boardId, Guid columnId)
    {
        return await dbContext.Columns
            .Include(column => column.Cards)
            .FirstOrDefaultAsync(column => column.BoardId == boardId && column.Id == columnId);
    }
}