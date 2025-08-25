using System;

namespace API.DTOs;

public class ProcedureDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Order { get; set; }
}
