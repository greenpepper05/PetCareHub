using Core.Models;

namespace Core.Entities;

public class ServiceRecordProcedures : BaseEntity
{
    public int PetId { get; set; }
    public Pet? Pet { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }

    public int ClinicId { get; set; }
    public Clinic? Clinic { get; set; }

    public DateTime DateOfService { get; set; }

    public string? Notes { get; set; }

    public VisitType VisitType { get; set; }

    public decimal? Price { get; set; }

    public int? StaffId { get; set; }
    public Staff? Staff { get; set; }

    public string Status { get; set; } = ServiceStatus.Scheduled;

    public ICollection<ServiceRecordProcedureStep> ProcedureSteps { get; set; } = [];
}
