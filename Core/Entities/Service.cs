namespace Core.Entities;

public class Service : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
}
