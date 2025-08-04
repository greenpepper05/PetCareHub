using Core.Models;

namespace Core.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = AppointmentStatus.Pending;

    public int PetId { get; set; }
    public Pet? Pet { get; set; }

    public required string OwnerId { get; set; }
    public AppUser? Owner { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public int ClinicId { get; set; }
    public Clinic? Clinic { get; set; }

    public string? Notes { get; set; }

    public ICollection<ServiceProcedure> Procedures { get; set; } = new List<ServiceProcedure>();
    public ICollection<ProcedureStep> ProcedureSteps { get; set; } = new List<ProcedureStep>();
}
