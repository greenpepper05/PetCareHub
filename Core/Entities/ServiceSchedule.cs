namespace Core.Entities;

public class ServiceSchedule : BaseEntity
{
    public string ServiceName { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public int IntervalInDays { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }
}
