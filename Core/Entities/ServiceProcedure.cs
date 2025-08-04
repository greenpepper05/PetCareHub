namespace Core.Entities;

public class ServiceProcedure : BaseEntity
{
    public string StepName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;

    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
}
