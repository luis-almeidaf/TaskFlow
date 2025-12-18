using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Guid CreatedById { get; set; }
    [InverseProperty("CreatedBoards")] 
    public User CreatedBy { get; set; } = null!;
    
    public ICollection<BoardMember> Members { get; set; } = new HashSet<BoardMember>();
    public ICollection<Column> Columns { get; set; } = new List<Column>();
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}