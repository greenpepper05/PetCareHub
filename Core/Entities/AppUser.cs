using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address? Address { get; set; }
    public string? Contact { get; set; }

    public int? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }

    public ICollection<Pet>? Pets { get; set; } = [];
    public ICollection<Appointment>? Appointments { get; set; } = [];
}
