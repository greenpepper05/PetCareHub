using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    [Required]
    public string ServiceName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    [Required]
    public int PetId { get; set; }
    public string? Notes { get; set; }
    [Required]
    public string OwnerId { get; set; } = string.Empty;
    public int ClinicId { get; set; }
}
