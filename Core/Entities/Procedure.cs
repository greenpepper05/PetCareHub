namespace Core.Entities;

public class Procedure : BaseEntity
{
    public int ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StepOrder { get; set; }
}
