namespace Core.Entities;

public class Staff : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;

    public ICollection<ServiceRecordProcedures> ServiceRecords { get; set; } = [];

    public string? UserId { get; set; }

}
