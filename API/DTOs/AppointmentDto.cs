using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Appointment date is required")]
    public DateTime AppointmentDate { get; set; }
    [Required(ErrorMessage = "Service is required")]
    public int ServiceId { get; set; }
    [Required(ErrorMessage = "Pet is required")]
    public int PetId { get; set; }
    public string? Notes { get; set; }
    [Required(ErrorMessage = "Owner is required")]
    public string OwnerId { get; set; } = string.Empty;
    [Required(ErrorMessage = "Clinic is required")]
    public int ClinicId { get; set; }
}
