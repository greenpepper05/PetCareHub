namespace Core.Entities;

public class Pet : BaseEntity
{
    public required string Name { get; set; }
    public required string Breed { get; set; }
    public required string Species { get; set; }
    public required DateTime Birthdate { get; set; }
    public required string Gender { get; set; }
    public required string OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<ServiceRecord> ServiceRecords { get; set; } = [];

}
