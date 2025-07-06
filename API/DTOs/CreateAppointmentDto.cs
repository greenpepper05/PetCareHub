using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateAppointmentDto
{
    [Required]
    public string ServiceName { get; set; } = string.Empty;
    [Required]
    public DateTime AppointmentDate { get; set; }
    [Required]
    public string PetId { get; set; } = string.Empty;
    [Required]
    public string OwnerId { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
