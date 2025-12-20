using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.Card;
using TaskFlow.Domain.Repositories.Column;
using TaskFlow.Domain.Repositories.RefreshToken;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Infrastructure.DataAccess;
using TaskFlow.Infrastructure.DataAccess.Repositories;
using TaskFlow.Infrastructure.Extensions;
using TaskFlow.Infrastructure.Identity;
using TaskFlow.Infrastructure.Security.Tokens;

namespace TaskFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddBcrypt(services);
        AddUserRetriever(services);
        AddRepositories(services);
        AddToken(services, configuration);

        if (!configuration.IsTestEnvironment())
        {
            AddDbContext(services, configuration);
        }
    }

    private static void AddBcrypt(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncrypter, Security.Cryptography.BCrypt>();
    }

    private static void AddUserRetriever(IServiceCollection services)
    {
        services.AddScoped<IUserRetriever, UserRetriever>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(("Connection"));
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<TaskFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();
        services.AddScoped<IBoardWriteOnlyRepository, BoardRepository>();
        services.AddScoped<IBoardReadOnlyRepository, BoardRepository>();
        services.AddScoped<IColumnReadOnlyRepository, ColumnRepository>();
        services.AddScoped<IColumnWriteOnlyRepository, ColumnRepository>();
        services.AddScoped<ICardReadOnlyRepository, CardRepository>();
        services.AddScoped<ICardWriteOnlyRepository, CardRepository>();
    }
}