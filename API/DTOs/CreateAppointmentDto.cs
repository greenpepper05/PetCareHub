using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateAppointmentDto
{
    [Required(ErrorMessage = "Service is required")]
    public int ServiceId { get; set; }
    [Required(ErrorMessage = "Appointment date is required")]
    public DateTime AppointmentDate { get; set; }
    [Required(ErrorMessage = "Pet is required")]
    public int PetId { get; set; }
    [Required(ErrorMessage = "Owner is required")]
    public string OwnerId { get; set; } = string.Empty;
    public string? Notes { get; set; }
    [Required(ErrorMessage = "Clinic is Required")]
    public int ClinicId { get; set; }
}
