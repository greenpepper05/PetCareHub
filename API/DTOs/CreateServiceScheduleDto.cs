using System;

namespace API.DTOs;

public class CreateServiceScheduleDto
{
    public string ServiceName { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public int IntervalInDays { get; set; }
    public int ServiceId { get; set; }
}
