using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.IntegrationTests.Resources;

public class ColumnIdentityManager
{
    private readonly Column _column;

    public ColumnIdentityManager(Column column)
    {
        _column = column;
    }

    public Guid GetId() => _column.Id;
}