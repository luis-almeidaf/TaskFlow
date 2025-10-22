using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.DataAccess;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}