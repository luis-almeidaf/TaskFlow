using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.User;

namespace TaskFlow.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly TaskFlowDbContext _dbContext;

    public UserRepository(TaskFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var userToRemove = await _dbContext.Users.FindAsync(user.Id);
        _dbContext.Users.Remove(userToRemove!);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetById(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        return user;
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}