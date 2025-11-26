using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UpdateUserDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Contact { get; set; } = string.Empty;
    [Required]
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public int? ClinicId { get; set; }
}
