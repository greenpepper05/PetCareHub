using System;

namespace API.DTOs;

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}
