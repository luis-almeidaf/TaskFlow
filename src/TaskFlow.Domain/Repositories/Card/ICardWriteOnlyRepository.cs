namespace TaskFlow.Domain.Repositories.Card;

public interface ICardWriteOnlyRepository
{
    Task Add(Entities.Card card);
    Task<Entities.Card?> GetById(Entities.User user,Guid boardId, Guid columnId, Guid cardId);
    void Update(Entities.Card card);
}