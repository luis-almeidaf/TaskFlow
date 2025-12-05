using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Infrastructure.DataAccess;

namespace TaskFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly TaskFlowDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(TaskFlowDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Id == Guid.Parse(identifier));
    }

    public async Task<User> GetUserAndBoards()
    {
        var token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        var userId = Guid.Parse(identifier);

        return await _dbContext.Users
            .Include(u => u.CreatedBoards)
            .Include(u => u.Boards)
            .ThenInclude(b => b.CreatedBy)
            .FirstAsync(user => user.Id == userId);
    }
}