namespace API.DTOs;

public class ServiceProcedureDto
{
    public int Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

public class UpdateProcedureStatusDto
{
    public bool IsCompleted { get; set; }
}
