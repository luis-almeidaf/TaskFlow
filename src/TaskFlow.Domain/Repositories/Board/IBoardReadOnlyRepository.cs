namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardReadOnlyRepository
{
    Task<List<Entities.Board>> GetAll(Entities.User user);
}