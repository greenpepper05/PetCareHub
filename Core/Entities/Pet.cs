namespace Core.Entities;

public class Pet : BaseEntity
{
    public required string Name { get; set; }
    public required string Breed { get; set; }
    public required string Species { get; set; }
    public required DateTime Birthdate { get; set; }
    public required string Gender { get; set; }
    public required string OwnerId { get; set; }
    public int? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
}
