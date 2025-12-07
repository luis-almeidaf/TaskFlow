namespace TaskFlow.Domain.Repositories.Card;

public interface ICardWriteOnlyRepository
{
    Task Add(Entities.Card card);
    Task<Entities.Card?> GetById(Entities.User user,Guid boardId, Guid columnId, Guid cardId);
    Task<List<Entities.Card>> ReorderCards(Guid columnId, int position);
    void Update(Entities.Card card);
    void UpdateRange(IEnumerable<Entities.Card> cards);
    void Delete(Entities.Card card);
}