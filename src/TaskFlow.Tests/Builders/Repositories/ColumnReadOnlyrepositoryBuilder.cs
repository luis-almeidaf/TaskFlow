using Moq;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Column;

namespace TaskFlow.Tests.Builders.Repositories;

public class ColumnReadOnlyRepositoryBuilder
{
    private readonly Mock<IColumnReadOnlyRepository> _repository = new();
    
    public ColumnReadOnlyRepositoryBuilder GetById(Guid boardId,Column column, Guid? id = null)
    {
        if (id.HasValue)
        {
            _repository.Setup(repo => repo.GetById(boardId, id.Value)).ReturnsAsync((Column?)null);
        }
        else
        {
            _repository.Setup(repo => repo.GetById(boardId, column.Id)).ReturnsAsync(column);
        }

        return this;
    }
    
    public IColumnReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}