namespace Core.Entities;

public class ClinicSchedule : BaseEntity
{
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public bool IsOpen { get; set; } = true;
}
