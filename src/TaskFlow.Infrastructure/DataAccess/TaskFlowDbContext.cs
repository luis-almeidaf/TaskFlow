using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.DataAccess;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardMember> BoardMembers { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Board>()
            .HasOne(b => b.CreatedBy)
            .WithMany(u => u.CreatedBoards)
            .HasForeignKey(b => b.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Column>()
            .HasOne(c => c.Board)
            .WithMany(b => b.Columns)
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Card>()
            .HasOne(c => c.CreatedBy)
            .WithMany(u => u.CreatedCards)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.AssignedTo)
            .WithMany(u => u.AssignedCards)
            .HasForeignKey(c => c.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Card>()
            .HasOne(c => c.Column)
            .WithMany(col => col.Cards)
            .HasForeignKey(c => c.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardMember>(entity =>
        {
            entity.HasKey(bm => bm.Id);
            
            entity.HasOne(bm => bm.Board)
                .WithMany(b => b.Members)
                .HasForeignKey(bm => bm.BoardId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(bm => bm.User)
                .WithMany(u => u.Boards)
                .HasForeignKey(bm => bm.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            entity.ToTable("BoardMembers");
        });
        
    }
}