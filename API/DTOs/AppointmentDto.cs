using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int PetId { get; set; }
    public string PetName { get; set; } = string.Empty;
    public string PetBreed { get; set; } = string.Empty;
    public string PetSpecies { get; set; } = string.Empty;
    public string PetGender { get; set; } = string.Empty;
    public string PetBirthdate { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int ClinicId { get; set; }
    public string ClinicName { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? LastReminderSent { get; set; }

}
