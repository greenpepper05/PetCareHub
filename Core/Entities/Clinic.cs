namespace Core.Entities;

public class Clinic : BaseEntity
{
    public string? OwnerId { get; set; }
    public required string ClinicName { get; set; }
    public string Address { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}
