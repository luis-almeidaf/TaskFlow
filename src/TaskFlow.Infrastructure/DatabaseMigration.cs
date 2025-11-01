using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure.DataAccess;

namespace TaskFlow.Infrastructure;

public static class DatabaseMigration
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<TaskFlowDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}