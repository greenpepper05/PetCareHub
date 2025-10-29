namespace API.DTOs;

public class PetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
}
