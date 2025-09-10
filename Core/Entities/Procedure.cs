namespace Core.Entities;

public class Procedure : BaseEntity
{
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Order { get; set; }
  
}
