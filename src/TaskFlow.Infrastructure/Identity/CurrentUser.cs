using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Infrastructure.DataAccess;

namespace TaskFlow.Infrastructure.Identity;

public class CurrentUser(TaskFlowDbContext dbContext, ITokenProvider tokenProvider) : ICurrentUser
{
    public async Task<User> GetCurrentUser()
    {
        var token = tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await dbContext.Users.AsNoTracking().FirstAsync(user => user.Id == Guid.Parse(identifier));
    }
}