namespace API.DTOs;

public class StaffDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string StaffRole { get; set; } = null!;
    public string? PictureUrl { get; set; }
    public int ClinicId { get; set; }
}
