using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskFlow.Application.UseCases.Login;

namespace TaskFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<ILoginUseCase, LoginUseCase>();
    }
}