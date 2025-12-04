using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Card;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class CardRepository(TaskFlowDbContext dbContext) : ICardReadOnlyRepository
{
    public async Task<Card?> GetCardById(User user,Guid boardId, Guid columnId, Guid cardId)
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Include(card => card.CreatedBy)
            .Include(card => card.AssignedTo)
            .Where(card => card.Column.Board.CreatedById == user.Id || card.Column.Board.Users.Any(u => u.Id == user.Id))
            .Where(card => card.Id == cardId)
            .Where(card => card.ColumnId == columnId)
            .Where(card => card.Column.BoardId == boardId)
            .FirstOrDefaultAsync();
    }
}