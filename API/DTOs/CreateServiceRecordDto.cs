using System;
using Core.Entities;

namespace API.DTOs;

public class CreateServiceRecordDto
{
    public string OwnerId { get; set; } = string.Empty;
    public int PetId { get; set; }
    public int ServiceId { get; set; }
    public int ClinicId { get; set; }
    public decimal? Price { get; set; }
    public DateTime DateOfService { get; set; }
    public string? Notes { get; set; }
    public VisitType VisitType { get; set; }
    public int? AppointmentId { get; set; }
}
