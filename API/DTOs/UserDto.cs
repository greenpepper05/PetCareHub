namespace API.DTOs;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Contact { get; set; } = null!;
    public string? Role { get; set; } = null!;

    public int? ClinicId { get; set; }
    public string? ClinicName { get; set; }
}
