using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Domain.Services.LoggedUser;
using TaskFlow.Infrastructure.DataAccess;
using TaskFlow.Infrastructure.DataAccess.Repositories;
using TaskFlow.Infrastructure.Extensions;
using TaskFlow.Infrastructure.Security.Tokens;
using TaskFlow.Infrastructure.Services.LoggedUser;

namespace TaskFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddBcrypt(services);
        AddLoggedUser(services);
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

    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
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
        services.AddScoped<IBoardWriteOnlyRepository, BoardRepository>();
    }
}