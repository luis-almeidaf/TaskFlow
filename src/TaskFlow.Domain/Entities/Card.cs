using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Domain.Entities;

public class Card
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    
    [InverseProperty("CreatedCards")]
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public Guid? AssignedToId { get; set; }
    [InverseProperty("AssignedCards")]
    public User? AssignedTo { get; set; }
    public Guid ColumnId { get; set; }
    public Column Column { get; set; } = null!;
}