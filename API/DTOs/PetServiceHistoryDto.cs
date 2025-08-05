using System;

namespace API.DTOs;

public class PetServiceHistoryDto
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public string PetName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string ClinicName { get; set; } = string.Empty;
    public DateTime DateOfService { get; set; }
    public string? Notes { get; set; } = string.Empty;
    public string VisitType { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
}
