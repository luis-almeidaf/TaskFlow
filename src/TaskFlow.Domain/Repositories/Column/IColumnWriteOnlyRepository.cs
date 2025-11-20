namespace TaskFlow.Domain.Repositories.Column;

public interface IColumnWriteOnlyRepository
{
    Task Add(Entities.Column column);
}
