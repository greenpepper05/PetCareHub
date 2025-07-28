namespace Core.Entities;

public class Appointment : BaseEntity
{
    public required string ServiceName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "Pending";
    public int PetId { get; set; }
    public Pet? Pet { get; set; }
    public required string OwnerId { get; set; }
    public string? Notes { get; set; }

    public int ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
}
