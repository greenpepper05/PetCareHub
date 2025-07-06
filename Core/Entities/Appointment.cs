namespace Core.Entities;

public class Appointment : BaseEntity
{
    public required string ServiceName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string? PetId { get; set; }
    public required string OwnerId { get; set; }
    public string? Notes { get; set; }
}
