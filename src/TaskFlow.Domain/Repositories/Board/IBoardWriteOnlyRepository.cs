namespace TaskFlow.Domain.Repositories.Board;

public interface IBoardWriteOnlyRepository
{
    Task Add(Entities.Board board);
    void AddUserToBoard(Entities.Board board, Entities.User user);
}