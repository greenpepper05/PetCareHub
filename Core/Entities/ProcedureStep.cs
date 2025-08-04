namespace Core.Entities;

public class ProcedureStep : BaseEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public string StepName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;

    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
