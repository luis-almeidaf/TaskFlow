using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Card;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class CardRepository(TaskFlowDbContext dbContext) : ICardReadOnlyRepository, ICardWriteOnlyRepository
{
    async Task<Card?> ICardReadOnlyRepository.GetById(User user, Guid boardId, Guid columnId, Guid cardId)
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Include(card => card.CreatedBy)
            .Include(card => card.AssignedTo)
            .Where(card =>
                card.Column.Board.CreatedById == user.Id || card.Column.Board.Users.Any(u => u.Id == user.Id))
            .Where(card => card.Id == cardId)
            .Where(card => card.ColumnId == columnId)
            .Where(card => card.Column.BoardId == boardId)
            .FirstOrDefaultAsync();
    }

    public async Task Add(Card card)
    {
        await dbContext.Cards.AddAsync(card);
    }

    async Task<Card?> ICardWriteOnlyRepository.GetById(User user, Guid boardId, Guid columnId, Guid cardId)
    {
        return await dbContext.Cards
            .Where(card =>
                card.Id == cardId &&
                card.ColumnId == columnId &&
                card.Column.BoardId == boardId &&
                (card.Column.Board.CreatedById == user.Id ||
                 card.Column.Board.Users.Any(u => u.Id == user.Id)))
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Card>> ReorderCards(Guid columnId, int position)
    {
        var cards = await dbContext.Cards
            .Where(card => card.ColumnId == columnId)
            .Where(c => c.Position > position).ToListAsync();

        foreach (var card in cards)
        {
            card.Position--;
        }

        return cards;
    }

    public void Update(Card card)
    {
        dbContext.Cards.Update(card);
    }

    public void UpdateRange(IEnumerable<Card> cards)
    {
        dbContext.Cards.UpdateRange(cards);
    }

    public void Delete(Card card)
    {
        dbContext.Remove(card);
    }
}