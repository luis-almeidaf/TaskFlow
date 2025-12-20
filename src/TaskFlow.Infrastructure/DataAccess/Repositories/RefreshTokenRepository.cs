using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.RefreshToken;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository(TaskFlowDbContext dbContext) : IRefreshTokenReadOnlyRepository, IRefreshTokenWriteOnlyRepository 
{
    public async Task Add(RefreshToken refreshToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task Delete(Guid userId)
    {
        var tokens =await dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ToListAsync();
        
        dbContext.RefreshTokens.RemoveRange(tokens);
    }

    public async Task<RefreshToken?> GetToken(string refreshToken)
    {
        return await dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }
}