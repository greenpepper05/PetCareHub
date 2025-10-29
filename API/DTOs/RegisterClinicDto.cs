namespace API.DTOs;

public class RegisterClinicDto
{
    public string OwnerId { get; set; } = string.Empty;
    public string ClinicName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PictureUrl { get; set; }
}
