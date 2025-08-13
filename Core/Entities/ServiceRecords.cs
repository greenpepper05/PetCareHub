namespace Core.Entities;

public class ServiceRecords : BaseEntity
{
    public int PetId { get; set; }
    public Pet? Pet { get; set; }
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public DateTime DatePerformed { get; set; }
    public string? Notes { get; set; }
}
