using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreatePetDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Breed { get; set; } = string.Empty;
    [Required]
    public string Species { get; set; } = string.Empty;
    [Required]
    public DateTime Birthdate { get; set; }
    [Required]
    public string Gender { get; set; } = string.Empty;
    [Required]
    public string OwnerId { get; set; } = string.Empty;
    public string? PictureUrl { get; set; }

}
