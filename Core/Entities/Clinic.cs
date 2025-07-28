namespace Core.Entities;

public class Clinic : BaseEntity
{
    public string? OwnerId { get; set; }
    public required string ClinicName { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}
