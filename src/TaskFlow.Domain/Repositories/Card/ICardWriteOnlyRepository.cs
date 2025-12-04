namespace TaskFlow.Domain.Repositories.Card;

public interface ICardWriteOnlyRepository
{
    Task Add(Entities.Card card);
}