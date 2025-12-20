namespace TaskFlow.Domain.Repositories.RefreshToken;

public interface IRefreshTokenWriteOnlyRepository
{
    Task Add(Entities.RefreshToken refreshToken);
    Task Delete(Guid userId);
}