namespace TaskFlow.Domain.Repositories.Card;

public interface ICardReadOnlyRepository
{
    Task<Entities.Card?> GetById(Entities.User user,Guid boardId, Guid columnId, Guid cardId);
}