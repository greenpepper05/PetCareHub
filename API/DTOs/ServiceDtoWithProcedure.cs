using System;

namespace API.DTOs;

public class ServiceDtoWithProcedure
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int ClinicId { get; set; }
    public IReadOnlyList<ProcedureDto> Procedures { get; set; } = [];
}
