namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardReadOnlyRepository
{
    Task<List<Entities.Board>> GetAll(Entities.User user);
    Task<Entities.Board?> GetById(Entities.User user, Guid id);
    Task<Entities.Column?> GetColumnById(Guid id);
}