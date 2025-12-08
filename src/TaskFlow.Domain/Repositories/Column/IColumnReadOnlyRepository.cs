namespace TaskFlow.Domain.Repositories.Column;

public interface IColumnReadOnlyRepository
{
    Task<Entities.Column?> GetById(Guid id);
}