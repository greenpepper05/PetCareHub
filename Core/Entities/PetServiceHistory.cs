namespace Core.Entities;

public class PetServiceHistory : BaseEntity
{
    public int PetId { get; set; }
    public Pet Pet { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }

    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; }
    public int? AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public DateTime DataOfService { get; set; }
    public string Notes { get; set; }
    public VisitType VisitType { get; set; }
}
