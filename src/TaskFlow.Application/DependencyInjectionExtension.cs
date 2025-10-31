using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}