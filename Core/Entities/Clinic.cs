namespace Core.Entities;

public class Clinic : BaseEntity
{
    public string? OwnerId { get; set; }
    public AppUser? Owner { get; set; }
    public required string ClinicName { get; set; }
    public string Address { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? PictureUrl { get; set; }
    public string? Status { get; set; }
    public bool IsOpen { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ClinicSchedule> Schedules { get; set; } = [];

    public ICollection<Service> Services { get; set; } = [];
    public ICollection<Staff> StaffMembers { get; set; } = [];
    public ICollection<AppUser> Staff { get; set; } = [];
    public ICollection<ServiceRecord> ServiceRecords { get; set; } = [];

}
